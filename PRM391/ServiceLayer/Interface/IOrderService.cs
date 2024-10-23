using BusinessLayer.Entity;
using BusinessLayer.Request;
using BusinessLayer.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interface
{
    public interface IOrderService
    {
        Task<OrderResponse> ConvertCartToOrder(int userId, OrderRequest orderRequest);
        Task<List<OrderResponse>> HistoryOrderByUser(int userId);
        
    }
}
