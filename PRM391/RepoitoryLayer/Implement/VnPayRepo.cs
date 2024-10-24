using BusinessLayer;
using BusinessLayer.Context;
using BusinessLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Repository
{
    public class VnPayRepo : IVnPayRepo
    {
        private readonly KoiContext _context;

        public VnPayRepo(KoiContext context)
        {
            _context = context;
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders.FindAsync(orderId);
        }

        
    }
}
