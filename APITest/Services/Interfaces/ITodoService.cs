using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APITest.Models;

namespace APITest.Services
{
    public interface ITodoService
    {
        TodoItem Get(string id);
        IEnumerable<TodoItem> GetAll();
        TodoItem Update(string id, TodoItem todo);
        void Delete(string id);
        TodoItem Create(TodoItem todo);
    }
}
