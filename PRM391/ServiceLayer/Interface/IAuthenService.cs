using BusinessLayer.Entity;
using BusinessLayer.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interface
{
    public interface IAuthenService
    {
        Task<string?> Login(LoginRequest loginRequest);
        Task<User> Register(RegisterRequest registerRequest);
        Task<User> GetUserById(int userID);
    }
}
