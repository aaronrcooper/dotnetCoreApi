using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Exceptions.NotFound
{
    public class UserNotFoundException : NotFoundException
    {
        public UserNotFoundException(string id) : base("User", "Id", id) { }
    }
}
