using BusinessLayer.Response;
using ServiceLayer.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Entity;
using RepoitoryLayer.Interface;
using RepoitoryLayer.Implement;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
namespace ServiceLayer.Implement
{
    public class CartService : ICartService
    {
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICartRepository _cartRepository;

        public CartService(IUserRepository userRepository, IProductRepository productRepository, ICartRepository cartRepository)
        {
            _userRepository = userRepository;
            _productRepository = productRepository;
            _cartRepository = cartRepository;
        }

        public async Task<CartResponse> AddToCart(int UserID, int ProductID, int Quantity)
        {
            User user = await _userRepository.GetUserById(UserID);
            if (user == null) return null;

            var product = await _productRepository.GetProductById(ProductID);
            if (product == null) return null;


            var existing = await _cartRepository.FindCartByUserIDAndProductIDWithStatusFalse(UserID, ProductID,false);
/*            var existingCart = existing?.FirstOrDefault(c => c.ProductId == ProductID && c.Status == false);
*/
            if (existing != null) { 
                /*if (existing.ProductId == ProductID && existing.Status == false)
                {*/
                    existing.Quantity += Quantity;
                    existing.Price = existing.Quantity * existing.Price;
                    _cartRepository.UpdateToCart(existing);

               /* }*/
               /* else
                {
                    Cart newCart = new Cart
                    {
                        UserId = UserID,
                        ProductId = ProductID,
                        Quantity = Quantity,
                        Price = (double)(Quantity * product.Price),
                        Status = false
                    };
                    _cartRepository.AddToCart(newCart);
                    var cartResponse = new CartResponse
                    {
                        CartId = newCart.CartId,
                        UserId = newCart.UserId,
                        ProductId = newCart.ProductId,
                        ProductName = product.ProductName,
                        Price = newCart.Price,
                        Quantity = newCart.Quantity,
                    };
                    return cartResponse;
                }*/
            }
            else
            {
                Cart newCart = new Cart
                {
                    UserId = UserID,
                    ProductId = ProductID,
                    Quantity = Quantity,
                    Price = (double)(Quantity * product.Price),
                    Status = false
                };
                _cartRepository.AddToCart(newCart);
                var cartResponse = new CartResponse
                {
                    CartId = newCart.CartId,
                    UserId = newCart.UserId,
                    ProductId = newCart.ProductId,
                    ProductName = product.ProductName,
                    Price = newCart.Price,
                    Quantity = newCart.Quantity,
                };

                return cartResponse;
            }
            var existingResponse = new CartResponse
            {
                CartId = existing.CartId,
                UserId = existing.UserId,
                ProductId = existing.ProductId,
                ProductName = product.ProductName,
                Price = existing.Price,
                Quantity = existing.Quantity,
            };
            return existingResponse;
        }

        public Task<bool> DeleteCartByProductID(int userId, int productId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CartResponse>> GetCartByUserId(int userId)
        {
            User user = await _userRepository.GetUserById(userId);
            if (user == null) return null;

            var cartItems = await _cartRepository.FindCartsByUserIdAndStatus(userId, false);
            var cartResponseList = new List<CartResponse>();
            foreach (var cart in cartItems)
            {
                Product? product = await _productRepository.GetProduct(cart.ProductId);

                cartResponseList.Add(new CartResponse
                {
                    CartId = cart.CartId,
                    UserId = cart.UserId,
                    ProductId = cart.ProductId,
                    ProductName = product.ProductName != null ? product.ProductName : "",
                    Price = cart.Price,
                    Quantity = cart.Quantity,
                    Status = cart.Status,
                    Image = product.Image,
                });
            }

            return cartResponseList;
        }

    

        public async Task<bool> RemoveCart(int userId)
        {
            User user = await _userRepository.GetUserById(userId);
            if (user == null) return false;

            var cartItems = await _cartRepository.FindListCartByUserID(userId);
            var itemsToRemove = cartItems.Where(cart => cart.Status == false).ToList();
            if (itemsToRemove == null || !itemsToRemove.Any())
            {
                return false; // No cart items with Status == false to remove
            }
            foreach (var cartItem in itemsToRemove)
            {
                _cartRepository.RemoveCart(cartItem.CartId);
            }
            return true;
        }
        public async Task<bool> RemoveByCartID(int cartId)
        {
            var cartItem = await _cartRepository.GetCartById(cartId);

            if (cartItem == null)
            {
                return false;
            }

            _cartRepository.RemoveCart(cartId);
            return true; 
        }
        public async Task<bool> UpdateQuantityAsync(int cartId, int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
            }

            var cart = await _cartRepository.GetCartById(cartId);
            if (cart == null)
            {
                throw new KeyNotFoundException($"Cart with ID {cartId} not found.");
            }
            cart.Price = (cart.Price / cart.Quantity) * quantity;
            cart.Quantity = quantity;
            _cartRepository.UpdateCart(cart);
            return true;
        }
    }

}
