using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APITest.Exceptions.NotFound
{
    public class PersonNotFoundException : NotFoundException
    {
        public PersonNotFoundException(string id) : base("Person", "Id", id) { }
    }
}
