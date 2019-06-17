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
        Task<DTO.TodoItem> Get(Guid id);
        Task<IEnumerable<DTO.TodoItem>> GetAll();
        Task<DTO.TodoItem> Update(Guid id, TodoItem todo);
        void Delete(Guid id);
        Task<DTO.TodoItem> Create(TodoItem todo);
    }
}
