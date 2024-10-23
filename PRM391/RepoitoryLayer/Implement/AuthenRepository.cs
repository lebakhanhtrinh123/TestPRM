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
    public class AuthenRepository : IAuthenRepository
    {
        private readonly KoiContext _context;

        public AuthenRepository(KoiContext context)
        {
            _context = context;
        }
        public async Task<User> AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public async Task<User> findAccount(string existingEmail)
        {
            var user = _context.Users.FirstOrDefault(n => n.Email == existingEmail);
            return user;
        }

        public async Task<string> findByEmail(string email)
        {
            var user = _context.Users.FirstOrDefault(n => n.Email == email);
            if (user != null)
            {
                return user.Email;
            }

            return null;
        }
    }
}
