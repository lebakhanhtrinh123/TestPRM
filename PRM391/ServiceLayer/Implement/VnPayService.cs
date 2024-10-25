using BusinessLayer.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Interface;
using RepositoryLayer.Repository;
using ServiceLayer.Helpers;
using ServiceLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Service
{
    public class VnPayService : IVnPayService
    {
        private readonly IVnPayRepo _orderRepository;
        private readonly IConfiguration _configuration;

        public VnPayService(IVnPayRepo orderRepository, IConfiguration configuration)
        {
            _orderRepository = orderRepository;
            _configuration = configuration;
        }

        public async Task<string> CreatePaymentUrlAsync(int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                throw new Exception("Order not found");
            }

            var amount = order.TotalPrice;
            return GeneratePaymentUrl(orderId, (decimal)amount);
        }

        private string GeneratePaymentUrl(int orderId, decimal amount)
        {
            var tick = DateTime.Now.Ticks.ToString();

            var vnpay = new VnPayLibrary();            

            var ipAddr = "127.0.0.1"; //Config dai
            var createDate = DateTime.Now.ToString("yyyyMMddHHmmss");
            var orderIdStr = orderId.ToString();
            var amountStr = amount;

            // Retrieve VnPay settings directly from the configuration
            var vnp_TmnCode = _configuration["vnp_TmnCode"];
            var vnp_HashSecret = _configuration["vnp_HashSecret"];
            var vnp_Url = _configuration["vnp_Url"];
            var returnUrl = _configuration["vnp_ReturnUrl"];

            vnpay.AddRequestData("vnp_Version", "2.1.0");
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", ((int)amountStr * 100).ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000

            vnpay.AddRequestData("vnp_CreateDate", createDate);
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", ipAddr);
            vnpay.AddRequestData("vnp_Locale", "vn");

            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan cho ma GD:" + orderIdStr);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other
            vnpay.AddRequestData("vnp_ReturnUrl", returnUrl);

            vnpay.AddRequestData("vnp_TxnRef", orderIdStr);

            var paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);

            return paymentUrl;
            /*var vnp_Params = new SortedDictionary<string, string>
        {
            { "vnp_Version", "2.1.0" },
            { "vnp_Command", "pay" },
            { "vnp_TmnCode", vnp_TmnCode },
            { "vnp_Locale", "vn" },
            { "vnp_CurrCode", "VND" },
            { "vnp_TxnRef", orderIdStr },
            { "vnp_OrderInfo", "Thanh toan cho ma GD:" + orderIdStr },
            { "vnp_OrderType", "other" },
            { "vnp_Amount", ((int)amountStr * 100).ToString() },
            { "vnp_ReturnUrl", returnUrl },
            { "vnp_IpAddr", ipAddr },
            { "vnp_CreateDate", createDate }
        };

            var paymentUrl = new UriBuilder(vnp_Url);
            var query = string.Join("&", vnp_Params.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));
            paymentUrl.Query = query;

            // Generate the secure hash
            var secureHash = GenerateSecureHash(paymentUrl.Query, vnp_HashSecret);
            paymentUrl.Query += $"&vnp_SecureHash={secureHash}";

            return paymentUrl.ToString();*/
        }

        private string GenerateSecureHash(string data, string hashSecret)
        {
            using (var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(hashSecret)))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
        public VnPaymentResponseModel PaymentExecute(IQueryCollection collections)
        {
            var vnpay = new VnPayLibrary();
            foreach (var (key, value) in collections)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, value.ToString());
                }
            }

            var vnp_orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
            var vnp_TransactionId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
            var vnp_SecureHash = collections.FirstOrDefault(p => p.Key == "vnp_SecureHash").Value;
            var vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            var vnp_OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");

            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, _configuration["vnp_HashSecret"]);
            if (!checkSignature)
            {
                return new VnPaymentResponseModel
                {
                    Success = false
                };
            }

            return new VnPaymentResponseModel
            {
                Success = true,
                PaymentMethod = "VnPay",
                OrderDescription = vnp_OrderInfo,
                OrderId = vnp_orderId.ToString(),
                TransactionId = vnp_TransactionId.ToString(),
                Token = vnp_SecureHash,
                VnPayResponseCode = vnp_ResponseCode
            };
        }
    }
}
