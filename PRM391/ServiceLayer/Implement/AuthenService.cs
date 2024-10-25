using BusinessLayer.Entity;
using BusinessLayer.Request;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepoitoryLayer.Implement;
using RepoitoryLayer.Interface;
using ServiceLayer.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Implement
{
    public class AuthenService : IAuthenService
    {
        private readonly IAuthenRepository authenRepository;
        private IConfiguration configuration;
        private readonly IUserRepository userRepository;

        public AuthenService(IAuthenRepository authenRepository, IConfiguration configuration, IUserRepository userRepository)
        {
            this.authenRepository = authenRepository;
            this.configuration = configuration;
            this.userRepository = userRepository;
        }
        public async Task<string?> Login(LoginRequest loginRequest)
        {
            string existingEmail = await authenRepository.findByEmail(loginRequest.Email);
            if (existingEmail == null)
            {
                throw new Exception("tài khoản chưa kích hoạt");
            }
            User user = await authenRepository.findAccount(existingEmail);
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginRequest.PasswordHash, user.PasswordHash);
            if (isPasswordValid)
            {
                return await GenerateJwtToken(user);
            }
            return null;
        }

        public async Task<User> Register(RegisterRequest registerRequest)
        {

            string email = await authenRepository.findByEmail(registerRequest.Email);
            if (email != null)
            {
                throw new Exception("email đã được sử dụng");
            }
            User user = new User
            {
                Email = registerRequest.Email,
                Username = registerRequest.UserName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerRequest.PasswordHash),
                RoleId = 1,
                CreatedAt = DateTime.Now,
            };
            var addUser = await authenRepository.AddUser(user);
            return addUser;
        }
        public async Task<User> GetUserById(int userID)
        {
            return await userRepository.GetUserById(userID);
        }

        private async Task<string> GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration["jwt:Key"]);

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, user.Username),
        new Claim("userId", user.UserId.ToString()),
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        new Claim("RoleId", user.RoleId.ToString()), // Thêm RoleId vào claims
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Thêm JTI
    };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1), // Thời hạn hiệu lực của token
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token); // Trả về token dưới dạng chuỗi
        }
    }
}
