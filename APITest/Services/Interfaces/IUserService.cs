using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using APITest.Models;

namespace APITest.Services
{
    public interface IUserService
    {
        Task<User> Get(string id);
        Task<IEnumerable<User>> GetAll();
        Task<string> Update(string id, UserPut user);
        void Delete(string id);
        Task<User> Create(UserPost user);
        Task<bool> IsAdmin(User user);
        Task<string> Login(UserCredentials credentials);
        Task<string> GenerateToken(User user);
    }
}
