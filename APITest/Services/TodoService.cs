using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APITest.Models;

namespace APITest.Services
{
    public class TodoService : ITodoService
    {
        public TodoItem Create(TodoItem todo)
        {
            throw new NotImplementedException();
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public TodoItem Get(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TodoItem> GetAll()
        {
            throw new NotImplementedException();
        }

        public TodoItem Update(string id, TodoItem todo)
        {
            throw new NotImplementedException();
        }
    }
}
