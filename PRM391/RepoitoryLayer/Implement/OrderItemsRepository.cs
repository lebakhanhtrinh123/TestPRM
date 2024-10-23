using BusinessLayer.Context;
using BusinessLayer.Entity;
using BusinessLayer.Request;
using Microsoft.EntityFrameworkCore;
using RepoitoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoitoryLayer.Implement
{
    public class OrderItemsRepository : IOrderItemsRepository
    {
        private readonly KoiContext _context;

        public OrderItemsRepository(KoiContext context)
        {
            _context = context;
        }
        public async Task CreateOrderItem(OrderItem orderItem)
        {
            await _context.OrderItems.AddAsync(orderItem);
            await _context.SaveChangesAsync();
        }

        public async Task<List<OrderItem>> GetOrderItemsByOrderId(int orderId)
        {
            return await _context.OrderItems
           .Where(orderItem => orderItem.OrderId == orderId)
           .ToListAsync();
        }
    }
}
