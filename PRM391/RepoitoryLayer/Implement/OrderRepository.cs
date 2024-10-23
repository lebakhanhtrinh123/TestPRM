using BusinessLayer.Context;
using BusinessLayer.Entity;
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

        public OrderRepository(KoiContext context)
        {
            _context = context;
        }
        public void CreateOrder(Order order)
        {
             _context.Orders.Add(order);
             _context.SaveChanges();
        }

        /*     public async Task CreateOrder(Order order)
             {
                 await _context.Orders.AddAsync(order); // Add order to the context
                 await _context.SaveChangesAsync(); // Save changes
             }*/
        public async Task<Order> FindByOrderID(int orderId)
        {
            return await _context.Orders.FindAsync(orderId); // Fetch order by ID
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
