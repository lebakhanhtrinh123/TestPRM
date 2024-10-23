using BusinessLayer.Context;
using BusinessLayer.Entity;
using RepoitoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoitoryLayer.Implement
{
    public class UserRepository : IUserRepository
    {
        private readonly KoiContext _context;

        public UserRepository(KoiContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserById(int userId)
        {
           return _context.Users.FirstOrDefault(n => n.UserId == userId);
        }
    }
}
