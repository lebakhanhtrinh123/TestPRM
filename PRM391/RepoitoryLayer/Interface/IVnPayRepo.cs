using BusinessLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IVnPayRepo
    {
        Task<Order> GetOrderByIdAsync(int id);
    }
}
