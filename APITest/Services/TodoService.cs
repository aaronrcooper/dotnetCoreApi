using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APITest.Exceptions.BadRequest;
using APITest.Exceptions.NotFound;
using Microsoft.EntityFrameworkCore;
using DTO;
using APITest.Models;

namespace APITest.Services
{
    public class TodoService : ITodoService
    {
        private readonly TodoContext _context;

        public TodoService(TodoContext context)
        {
            _context = context;
        }

        public async Task<TodoItem> Create(TodoItem todo)
        {
            await _context.TodoItems.AddAsync(todo);
            await _context.SaveChangesAsync();
            return todo;
        }

        public async void Delete(string id)
        {
            var todo = await _context.TodoItems.SingleAsync(item => item.Id == id);

            if (todo == null)
            {
                throw new TodoNotFoundException(id);
            }

            _context.TodoItems.Remove(todo);
            await _context.SaveChangesAsync();
        }

        public async Task<TodoItem> Get(string id)
        {
            var todo = await _context.TodoItems.SingleAsync(item => item.Id == id);

            if (todo == null)
            {
                throw new TodoNotFoundException(id);
            }

            return todo;
        }

        public async Task<IEnumerable<TodoItem>> GetAll()
        {
            return await _context.TodoItems.ToListAsync();
        }

        public async Task<TodoItem> Update(string id, TodoItem todo)
        {
            if (todo.Id != id)
            {
                throw new BadRequestException("Todo", id);
            }

            _context.Entry(todo).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return todo;
        }
    }
}
