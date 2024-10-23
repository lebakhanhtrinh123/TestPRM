using BusinessLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoitoryLayer.Interface
{
    public interface IOrderRepository
    {
        void CreateOrder(Order order);
        Task<Order> FindByOrderID(int userId);
        Task<Order> UpdateOrder(Order order);
    }
}
