using BusinessLayer.Entity;
using BusinessLayer.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoitoryLayer.Interface
{
    public interface ICartRepository
    {
        void AddToCart(Cart newCart);
        Task<CartResponse> FindCartByUserID(int userID);
        void UpdateToCart(CartResponse existing);

        Task<List<Cart>> FindCartsByUserIdAndStatus(int userId, bool status);
        Task<List<Cart>> FindListCartByUserID(int userId);
        Task<bool> RemoveCart(int cartId);


        Task<List<Cart>> FindCartsByUserIDAndStatusFalse(int userID, bool v);
        void UpdateCart(Cart carItem);
        Task<Cart> GetCartById(int cartId);
        Task<CartResponse> FindCartByUserIDAndProductIDWithStatusFalse(int userID, int productID, bool v);
    }
}
