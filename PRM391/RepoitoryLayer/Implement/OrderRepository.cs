using BusinessLayer.Context;
using BusinessLayer.Entity;
using BusinessLayer.Response;
using Microsoft.EntityFrameworkCore;
using RepoitoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoitoryLayer.Implement
{
    public class OrderRepository : IOrderRepository
    {
        private readonly KoiContext _context;
        private readonly IProductRepository _productRepository;

        public OrderRepository(KoiContext context, IProductRepository productRepository)
        {
            _context = context;
            _productRepository = productRepository;
        }

        public void CreateOrder(Order order)
        {
             _context.Orders.Add(order);
             _context.SaveChanges();
        }

        public async Task<Order> FindByOrderID(int orderId)
        {
            return await _context.Orders.FindAsync(orderId); // Fetch order by ID
        }

        public async Task<List<OrderResponse>> FindOrderByUserID(int userId)
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems) 
                .Where(o => o.UserId == userId) 
                .ToListAsync();
            var orderResponses = orders.Select(order => new OrderResponse
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                TotalPrice = order.TotalPrice, 
                Address = order.Address, 
                Phone = order.Phone, 
                OrderItems = order.OrderItems.Select(oi => new OrderItermReponse
                {
                    productName = _productRepository.GetProductNameById(oi.CartId),
                    image = _productRepository.GetProductImageById(oi.CartId),

                }).ToList()
            }).ToList();

            return orderResponses;
        
         }

        public async Task<Order> UpdateOrder(Order order)
        {
            var existingOrder = await _context.Orders.FindAsync(order.OrderId);
            if (existingOrder == null)
            {
                throw new KeyNotFoundException($"Order with ID {order.OrderId} not found.");
            }

            existingOrder.TotalPrice = order.TotalPrice;
            existingOrder.Address = order.Address;
            existingOrder.Phone = order.Phone;
            await _context.SaveChangesAsync();
            return existingOrder;
        }
    }
}
