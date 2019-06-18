using APITest.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoItem = APITest.Domain.Models.TodoItem;


namespace Business.Services
{
    public interface ITodoService
    {
        Task<TodoItem> Get(Guid id);
        Task<IEnumerable<TodoItem>> GetAll();
        Task<TodoItem> Update(Guid id, TodoItem todo);
        void Delete(Guid id);
        Task<TodoItem> Create(TodoItem todo);
    }
}
