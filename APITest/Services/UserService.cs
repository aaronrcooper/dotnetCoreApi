using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APITest.Models;

namespace APITest.Services
{
    public class UserService : IUserService
    {
        public User Create(User user)
        {
            throw new NotImplementedException();
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public User Get(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<bool> isAdmin(TodoContext context, User user)
        {
            throw new NotImplementedException();
        }

        public User Update(string id, User user)
        {
            throw new NotImplementedException();
        }
    }
}
