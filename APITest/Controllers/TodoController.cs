using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Business.Exceptions.NotFound;
using System;
using Business.Exceptions.BadRequest;
using Business.Services;
using APITest.Domain.Models;
using AutoMapper;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APITest.Controllers
{
    [Route("api/[controller]")]
    public class TodoController : Controller
    {

        private readonly ITodoService TodoService;
        private readonly IMapper _mapper;

        public TodoController(ITodoService todoService, IMapper mapper)
        {
            TodoService = todoService;
            _mapper = mapper;
        }
        // GET: api/<controller>
        [HttpGet]
        [Produces(typeof(IEnumerable<DTO.TodoItem>))]
        public async Task<IActionResult> GetAllTodos()
        {
            return Ok(await TodoService.GetAll());
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        [Produces(typeof(DTO.TodoItem))]
        public async Task<IActionResult> GetTodoItem(Guid id)
        {
            try
            {
                var todo = await TodoService.Get(id);
                return Ok(_mapper.Map<TodoItem, DTO.TodoItem>(todo));
            }
            catch (TodoNotFoundException )
            {
                return BadRequest();
            }
        }

        // POST api/<controller>
        [HttpPost]
        [Produces(typeof(DTO.TodoItem))]
        public async Task<IActionResult> CreateTodo([FromBody] TodoItem value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var todo = await TodoService.Create(value);
                return CreatedAtAction(nameof(GetTodoItem), new { id = value.Id }, _mapper.Map<TodoItem, DTO.TodoItem>(value));
            }
            catch (BadRequestException )
            {
                return BadRequest();
            }
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Produces(typeof(DTO.TodoItem))]
        public async Task<IActionResult> EditTodo(Guid id, [FromBody] TodoItem value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var updatedTodo = await TodoService.Update(id, value);
                return Ok(_mapper.Map<TodoItem, DTO.TodoItem>(updatedTodo));
            }
            catch (BadRequestException)
            {
                return BadRequest();
            }
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public IActionResult DeleteTodo(Guid id)
        {
            try
            {
                TodoService.Delete(id);
                return NoContent();
            }
            catch (TodoNotFoundException )
            {
                return NotFound();
            }
        }
    }
}
