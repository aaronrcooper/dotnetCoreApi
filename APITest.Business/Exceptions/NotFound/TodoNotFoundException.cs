using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Exceptions.NotFound
{
    public class TodoNotFoundException : NotFoundException
    {
        public TodoNotFoundException(string id) : base("Todo", "Id", id) { }
    }
}
