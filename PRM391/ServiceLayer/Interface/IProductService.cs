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
    public interface IProductService
    {
        Task<List<ProductResponse>> GetAllProduct();
        Task<Product> CreateProduct(ProductRequest productRequest);
        Task<ProductResponse> UpdateProduct(int productID, ProductRequest productRequest);
        Task<Product> DeleteProduct(int productID);
    }
}
