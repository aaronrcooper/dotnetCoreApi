using System.Collections.Generic;
using System.Threading.Tasks;
using APITest.Exceptions.BadRequest;
using APITest.Exceptions.NotFound;
using APITest.Models;
using APITest.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APITest.Controllers
{
    [Route("api/[controller]")]
    public class TodoController : Controller
    {

        private readonly ITodoService TodoService;

        public TodoController(ITodoService todoService)
        {
            TodoService = todoService;
        }
        // GET: api/<controller>
        [HttpGet]
        [Produces(typeof(IEnumerable<TodoItem>))]
        public async Task<IActionResult> GetAllTodos()
        {
            return Ok(await TodoService.GetAll());
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        [Produces(typeof(TodoItem))]
        public async Task<IActionResult> GetTodoItem(string id)
        {
            try
            {
                var todo = await TodoService.Get(id);
                return Ok(todo);
            }
            catch (TodoNotFoundException )
            {
                return BadRequest();
            }
        }

        // POST api/<controller>
        [HttpPost]
        [Produces(typeof(TodoItem))]
        public async Task<IActionResult> CreateTodo([FromBody] TodoItem value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var todo = await TodoService.Create(value);
                return CreatedAtAction(nameof(GetTodoItem), new { id = value.Id }, value);
            }
            catch (BadRequestException )
            {
                return BadRequest();
            }
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Produces(typeof(TodoItem))]
        public async Task<IActionResult> EditTodo(string id, [FromBody] TodoItem value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var updatedTodo = await TodoService.Update(id, value);
                return Ok(updatedTodo);
            }
            catch (BadRequestException)
            {
                return BadRequest();
            }
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        [Produces(typeof(TodoItem))]
        public async Task<IActionResult> DeleteTodo(string id)
        {
            try
            {
                await Task.Run(() => TodoService.Delete(id));
                return NoContent();
            }
            catch (TodoNotFoundException )
            {
                return NotFound();
            }
        }
    }
}
