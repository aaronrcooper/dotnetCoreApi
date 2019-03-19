using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APITest.Models;

namespace APITest.Services
{
    public interface IUserService
    {
        Task<User> Get(string id);
        Task<IEnumerable<User>> GetAll();
        Task<User> Update(string id, UserPut user);
        void Delete(string id);
        Task<User> Create(UserPost user);
        Task<bool> IsAdmin(User user);
        Task<User> Login(UserCredentials credentials);
    }
}
