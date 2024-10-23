using BusinessLayer.Entity;
using BusinessLayer.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoitoryLayer.Interface
{
    public interface IProductRepository
    {
        Task<Product> CreateProduct(Product product);
        Task<Product> DeleteProduct(int productID);
        Task<List<ProductResponse>> GetAllProduct();
        Task<Product> GetProduct(int? productId);
        Task<ProductResponse> GetProductById(int productId);
        string GetProductImageById(int? cartId);
        string GetProductNameById(int? cartId);
        Task<ProductResponse> UpdateProduct(ProductResponse product);
    }
}
