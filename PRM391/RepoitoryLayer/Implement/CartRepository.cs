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
    public class CartRepository : ICartRepository
    {
        private readonly KoiContext _context;

        public CartRepository(KoiContext context)
        {
            _context = context;
        }

        public void AddToCart(Cart newCart)
        {
            _context.Carts.Add(newCart);
            _context.SaveChanges();
        }

        public async Task<  CartResponse> FindCartByUserID(int userID)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(n => n.UserId == userID);

            if (cart == null) return null;

            return new CartResponse
            {
                CartId = cart.CartId,
                ProductId = cart.ProductId,
                UserId = userID,
                Quantity = cart.Quantity,
                Price = cart.Price,
                Status = cart.Status,
            };

        }

        public async Task<List<Cart>> FindCartsByUserIdAndStatus(int userId, bool status)
        {
            return await _context.Carts
                    .Where(c => c.UserId == userId && c.Status == status)
                    .ToListAsync();
        }

        public async Task<List<Cart>> FindListCartByUserID(int userId)
        {
            return await _context.Carts
                .Where(cart => cart.UserId == userId)
                .ToListAsync();
        }

        public void UpdateToCart(CartResponse existing)
        {
            var cart = _context.Carts.FirstOrDefault(c => c.CartId == existing.CartId);
            if (cart != null)
            {
                cart.Quantity = existing.Quantity;
                cart.Price = existing.Price;
                cart.Status = existing.Status;

                _context.Carts.Update(cart);

                _context.SaveChanges();
            }
        }

        public async Task<bool> RemoveCart(int cartId)
        {
            var cartItem = _context.Carts.Find(cartId);
            if (cartItem != null)
            {
                _context.Carts.Remove(cartItem);
            }
            _context.SaveChanges();
            return true;

        }

        public async Task<List<Cart>> FindCartsByUserIDAndStatusFalse(int userID, bool v)
        {
            try
            {
                return await _context.Carts
            .Where(cart => cart.UserId == userID && cart.Status == v)
            .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateCart(Cart carItem)
        {
             _context.Carts.Update(carItem);
            _context.SaveChanges();

        }

        public async Task<Cart> GetCartById(int cartId)
        {
            // Retrieve the cart item from the database by CartId
            return await _context.Carts
                .AsNoTracking() // Optional: improves performance by not tracking the entity
                .FirstOrDefaultAsync(cart => cart.CartId == cartId);
        }

        public async Task<CartResponse> FindCartByUserIDAndProductIDWithStatusFalse(int userID, int productID, bool v)
        {
            var cart = await _context.Carts
                   .FirstOrDefaultAsync(c => c.UserId == userID && c.ProductId == productID && c.Status == v); if (cart == null) return null;
            return new CartResponse
            {
                CartId = cart.CartId,
                ProductId = cart.ProductId,
                UserId = userID,
                Quantity = cart.Quantity,
                Price = cart.Price,
                Status = cart.Status,
            };
        }
    }
}
