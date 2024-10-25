using BusinessLayer.Entity;
using BusinessLayer.Request;
using BusinessLayer.Response;
using Microsoft.Extensions.Configuration;
using RepoitoryLayer.Interface;
using ServiceLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Implement
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;
        private IConfiguration configuration;

        public ProductService(IProductRepository productRepository, IConfiguration configuration)
        {
            this.productRepository = productRepository;
            this.configuration = configuration;
        }

        public async Task<Product> CreateProduct(ProductRequest productRequest)
        {
            Product product = new Product
            {

                ProductDescription = productRequest.ProductDescription,
                ProductName = productRequest.ProductName,
                Price = productRequest.Price,
                StockQuantity = productRequest.StockQuantity,
                Image = productRequest.Image,
            };
            var products = await productRepository.CreateProduct(product);
            return products;
        }
        public async Task<ProductResponse> UpdateProduct(int productID, ProductRequest productRequest)
        {
            var product = await productRepository.GetProductById(productID);
            product.ProductName = productRequest.ProductName;
            product.Price = productRequest.Price;
            product.StockQuantity = productRequest.StockQuantity;
            product.Image = productRequest.Image;
            product.ProductDescription = productRequest.ProductDescription;
            var updateProduct = await productRepository.UpdateProduct(product);
            return updateProduct;
        }

        public async Task<List<ProductResponse>> GetAllProduct()
        {
            return await productRepository.GetAllProduct();
        }

        public async Task<Product> DeleteProduct(int productID)
        {
            return await productRepository.DeleteProduct(productID);
        }
    }
}
