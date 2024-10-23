using BusinessLayer.Entity;
using BusinessLayer.Request;
using BusinessLayer.Response;
using RepoitoryLayer.Implement;
using RepoitoryLayer.Interface;
using ServiceLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Implement
{
    public class OrderService : IOrderService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemsRepository _orderItemsRepository;
        private readonly IProductRepository _productRepository;

        public OrderService(IUserRepository userRepository, ICartRepository cartRepository, IOrderRepository orderRepository, IOrderItemsRepository orderItemsRepository, IProductRepository productRepository)
        {
            _userRepository = userRepository;
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;
            _orderItemsRepository = orderItemsRepository;
            _productRepository = productRepository;
        }



        /*      public async Task<Order> CreateOrder(int userID, OrderRequest orderRequest)
              {
                  User user = await _userRepository.GetUserById(userID);
                  if (user == null)
                  {
                      return null;
                  }
                  Order order = new Order();
                  order.UserId = userID;
                  order.OrderDate = DateTime.Now;
                  order.TotalPrice = 0;
                  order.Address = orderRequest.Address;
                  order.Phone = orderRequest.Phone;
                  await _orderRepository.CreateOrder(order);
                  List<Cart> cart = await _cartRepository.FindCartsByUserIDAndStatusFalse(userID,false);
                  foreach (var carItem in cart)
                  {
                      OrderItem orderItem = new OrderItem();
                      orderItem.OrderId = order.OrderId;
                      orderItem.CartId = carItem.CartId;
                      await _orderItemsRepository.CreateOrderItem(orderItem);
                  }
                  return order;

              }*/
       

        public async Task<OrderResponse> ConvertCartToOrder(int userId, OrderRequest orderRequest)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null)
            {
                return null;
            }
            Order order = new Order(); 
            order.UserId = userId;
            order.OrderDate = DateTime.Now;
            order.TotalPrice = 0; 
            order.Address = orderRequest.Address;
            order.Phone = orderRequest.Phone;
            _orderRepository.CreateOrder(order); //return order
            decimal TTprice = 0;

            /* var cartItmems = await _cartRepository.FindCartByUserID(userId);
             var cartItemssss = cartItmems.Status.Equals(false);*/
/*            List<Cart> carts = new();
*/            List<Cart> carts = await _cartRepository.FindCartsByUserIDAndStatusFalse(userId, false);
            //return list : chay ngam 

            
            foreach (var carItem in carts)
            {
                OrderItem orderItem = new OrderItem();
                orderItem.OrderId = order.OrderId;
                orderItem.CartId = carItem.CartId;
                await _orderItemsRepository.CreateOrderItem(orderItem);
                carItem.Status = true;
                _cartRepository.UpdateCart(carItem);
                TTprice += (decimal)carItem.Price;

            }
            var ordernew = await _orderRepository.FindByOrderID(order.OrderId);

            order.TotalPrice = TTprice;
            await _orderRepository.UpdateOrder(order);
            var orderItems = await _orderItemsRepository.GetOrderItemsByOrderId(order.OrderId);

            return new OrderResponse
            {
                OrderId = order.OrderId,
                UserId = userId,
                OrderDate = DateTime.Now,   
                TotalPrice = TTprice,
                Address = orderRequest.Address,
                Phone = orderRequest.Phone,
                OrderItems = orderItems.Select(oi => new OrderItermReponse
                {
                    productName = _productRepository.GetProductNameById(oi.CartId),
                    
                    
                }).ToList(),
            };

        }

        public async Task<List<OrderResponse>> HistoryOrderByUser(int userId)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null)
            {
                return null;
            }
            List<OrderResponse> order = await _orderRepository.FindOrderByUserID(userId);
            return order; 

        }
    }
}
