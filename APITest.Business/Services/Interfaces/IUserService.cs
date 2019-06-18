using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using APITest.Domain.Models;

namespace Business.Services
{
    public interface IUserService
    {
        Task<User> Get(Guid id);
        Task<IEnumerable<User>> GetAll();
        Task<LoggedInUser> Update(Guid id, UserPut user);
        void Delete(Guid id);
        Task<User> Create(UserPost user);
        Task<bool> IsAdmin(User user);
        Task<LoggedInUser> Login(UserCredentials credentials);
        Task<string> GenerateToken(User user);
    }
}
