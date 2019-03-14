using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APITest.Models;

namespace APITest.Services
{
    public interface IUserService
    {
        User Get(string id);
        IEnumerable<User> GetAll();
        User Update(string id, User user);
        void Delete(string id);
        User Create(User user);
    }
}
