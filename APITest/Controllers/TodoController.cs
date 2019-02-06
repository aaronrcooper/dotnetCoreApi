using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APITest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APITest.Controllers
{
    [Route("api/[controller]")]
    public class TodoController : Controller
    {

        private readonly TodoContext _context;

        public TodoController(TodoContext context)
        {
            _context = context;
        }
        // GET: api/<controller>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> Get()
        {
            return await  _context.TodoItems.ToListAsync();
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(string id)
        {
            var todo = await _context.TodoItems.SingleAsync(item => item.Id == id);

            if (todo == null)
            {
                return NotFound();
            }

            return todo;
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<ActionResult<TodoItem>> Post([FromBody]TodoItem value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _context.TodoItems.Add(value);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTodoItem), new { id = value.Id}, value);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<TodoItem>> Put(string id, [FromBody]TodoItem value)
        {
            if (value.Id != id )
            {
                return BadRequest();
            }

            _context.Entry(value).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TodoItem>> Delete(string id)
        {
            var todo = await _context.TodoItems.SingleAsync(item => item.Id == id);

            if (todo == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todo);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
