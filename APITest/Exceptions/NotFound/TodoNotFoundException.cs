using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APITest.Exceptions.NotFound
{
    public class TodoNotFoundException : NotFoundException
    {
        public TodoNotFoundException(string id) : base("Todo", "Id", id) { }
    }
}
