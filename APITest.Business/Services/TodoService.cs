using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Exceptions.BadRequest;
using Business.Exceptions.NotFound;
using Microsoft.EntityFrameworkCore;
using APITest.Domain;
using APITest.Domain.Models;

namespace Business.Services
{
    public class TodoService : ITodoService
    {
        private readonly TodoContext _context;

        public TodoService(TodoContext context)
        {
            _context = context;
        }

        public async Task<DTO.TodoItem> Create(TodoItem todo)
        {
            await _context.TodoItems.AddAsync(todo);
            await _context.SaveChangesAsync();
            return todo;
        }

        public async void Delete(Guid id)
        {
            var todo = await _context.TodoItems.SingleAsync(item => item.Id == id);

            if (todo == null)
            {
                throw new TodoNotFoundException(id.ToString());
            }

            _context.TodoItems.Remove(todo);
            await _context.SaveChangesAsync();
        }

        public async Task<DTO.TodoItem> Get(Guid id)
        {
            var todo = await _context.TodoItems.SingleAsync(item => item.Id == id);

            if (todo == null)
            {
                throw new TodoNotFoundException(id.ToString());
            }

            return todo;
        }

        public async Task<IEnumerable<DTO.TodoItem>> GetAll()
        {
            return await _context.TodoItems.ToListAsync();
        }

        public async Task<DTO.TodoItem> Update(Guid id, TodoItem todo)
        {
            if (todo.Id != id)
            {
                throw new BadRequestException("Todo", id.ToString());
            }

            _context.Entry(todo).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return todo;
        }
    }
}
