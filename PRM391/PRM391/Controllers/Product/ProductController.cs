using BusinessLayer.Request;
using BusinessLayer.Response;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Interface;

namespace PRM391.Controllers.Product
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet("get-all-product")]
        public async Task<ActionResult<List<ProductResponse>>> GetAllProducts()
        {
            var products = await productService.GetAllProduct();

            if (products == null || !products.Any())
            {
                return NotFound("No products found.");
            }

            return Ok(products);
        }

        [HttpPost("create-product")]
        public async Task<ActionResult> CreateProduct(ProductRequest productRequest)
        {
            var products = await productService.CreateProduct(productRequest);
            return Ok(products);
        }

        [HttpPut("update-product")]
        public async Task<ActionResult<ProductResponse>> UpdateProduct(int productID, [FromBody] ProductRequest productRequest)
        {
            var products = await productService.UpdateProduct(productID, productRequest);
            return Ok(products);
        }
        [HttpDelete("delete-product")]
        public async Task<ActionResult> DeleteProduct(int productID)
        {
            var product = await productService.DeleteProduct(productID);
            return Ok(product);
        }
    }
}
