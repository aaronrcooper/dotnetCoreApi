using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTO;


namespace APITest.Services
{
    public interface ITodoService
    {
        Task<TodoItem> Get(string id);
        Task<IEnumerable<TodoItem>> GetAll();
        Task<TodoItem> Update(string id, TodoItem todo);
        void Delete(string id);
        Task<TodoItem> Create(TodoItem todo);
    }
}
