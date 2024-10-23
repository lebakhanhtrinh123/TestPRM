using BusinessLayer.Entity;
using BusinessLayer.Request;
using BusinessLayer.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoitoryLayer.Interface
{
    public interface IOrderItemsRepository
    {
        Task CreateOrderItem(OrderItem orderItem);
        Task<List<OrderItem>> GetOrderItemsByOrderId(int orderId);
    }
}
