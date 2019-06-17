using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DTO;
using User = APITest.Domain.Models.User;

namespace Business.Services
{
    public interface IUserService
    {
        Task<DTO.User> Get(Guid id);
        Task<IEnumerable<DTO.User>> GetAll();
        Task<string> Update(Guid id, UserPut user);
        void Delete(Guid id);
        Task<DTO.User> Create(UserPost user);
        Task<bool> IsAdmin(User user);
        Task<string> Login(UserCredentials credentials);
        Task<string> GenerateToken(User user);
    }
}
