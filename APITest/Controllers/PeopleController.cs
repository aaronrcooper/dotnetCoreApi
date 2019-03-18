using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APITest.Exceptions.NotFound;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using APITest.Models;
using APITest.Services;

namespace APITest.Controllers
{
    [Route("api/[controller]")]
    public class PeopleController : Controller
    {
        private IPersonService _personService;
        public PeopleController(IPersonService personService)
        {
            _personService = personService;
        }

        // GET: People
        [HttpGet]
        [Produces(typeof(List<Person>))]
        public async Task<IActionResult> Index()
        {
            return Ok(await _personService.GetAll());
        }

        // GET: People/Details/5
        [HttpGet("{id}")]
        [Produces(typeof(Person))]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                return Ok(await _personService.Get(id));
            }
            catch (PersonNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        // POST: People/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Produces(typeof(Person))]
        public async Task<IActionResult> Create([FromBody] Person person)
        {
            if (ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                await _personService.Create(person);
                return CreatedAtAction(nameof(Create), person);
            }
            catch (PersonNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut("{id}")]
        [Produces(typeof(Person))]
        public async Task<IActionResult> Edit(string id, [FromBody]Person person)
        {
            try
            {
                await _personService.Update(id, person);
            }
            catch (PersonNotFoundException e)
            {
                return NotFound(e.Message);
            }

            return Ok(person);
        }

        // POST: People/Delete/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                _personService.Delete(id);
                return Ok();
            }
            catch (PersonNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
