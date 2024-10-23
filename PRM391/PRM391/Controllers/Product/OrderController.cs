using Azure.Core;
using BusinessLayer.Request;
using BusinessLayer.Response;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Interface;

namespace PRM391.Controllers.Product
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : Controller
    {
        private readonly IOrderService orderService;

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpPost("create-order")]
        public async Task<ActionResult> CreateOrder(int userID, OrderRequest orderRequest)
        {
            var result = await orderService.ConvertCartToOrder(userID, orderRequest);
            return Ok(result);
        }
        [HttpGet("get-order")]
        public async Task<ActionResult<List<OrderResponse>>> GetOrder(int userId)
        {
            var result = await orderService.HistoryOrderByUser(userId);
            return Ok(result);
        }

    }
}
