using BusinessLayer.Response;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Implement;
using ServiceLayer.Interface;

namespace PRM391.Controllers.Product
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : Controller
    {
        private readonly ICartService cartService;

        public CartController(ICartService cartService)
        {
            this.cartService = cartService;
        }
        [HttpPost("add-to-cart")]
        public async Task<CartResponse> AddToCart( int userId, int ProductID , int Quantity)
        {
            var result = await cartService.AddToCart(userId, ProductID, Quantity);
            return result;
        }

        [HttpGet("get-cart")]
        public async Task<IActionResult> GetUserCart(int userId)
        {
            var cart = await cartService.GetCartByUserId(userId);
            if (cart == null || !cart.Any())
            {
                return NotFound();
            }

            return Ok(cart);
        }

        [HttpDelete("remove-cart")]
        public async Task<ActionResult<bool>> RemoveCart(int userID)
        {
            bool resutl = await cartService.RemoveCart(userID);  
            return Ok(resutl);
        }


        [HttpDelete("remove-cartID")]
        public async Task<ActionResult<bool>> RemoveByCartId( int cartId)
        {
            bool result = await cartService.RemoveByCartID(cartId);
            if (!result)
            {
                return NotFound("Cart item not found or does not belong to the user.");
            }

            return Ok(result); 
        }

        [HttpPut("updatequantity")]
        public async Task<ActionResult<bool>> UpdateQuantity(int cartId, int quantity)
        {
            bool result = await cartService.UpdateQuantityAsync(cartId, quantity);
            return Ok(result);

        }
    }
}
