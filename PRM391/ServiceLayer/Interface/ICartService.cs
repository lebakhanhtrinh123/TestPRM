using BusinessLayer.Entity;
using BusinessLayer.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interface
{
    public interface ICartService
    {
        Task<CartResponse> AddToCart(int userID, int ProductID, int Quantity);
        Task<List<CartResponse>> GetCartByUserId(int userId);
        Task<bool> RemoveCart(int userId);
        Task<bool> DeleteCartByProductID(int userId, int productId);
        Task<bool> RemoveByCartID( int cartId);

        Task<bool> UpdateQuantityAsync(int cartId, int quantity);

    }
}
