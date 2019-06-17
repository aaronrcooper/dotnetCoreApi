using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Exceptions.NotFound
{
    public class PersonNotFoundException : NotFoundException
    {
        public PersonNotFoundException(string id) : base("Person", "Id", id) { }
    }
}
