using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using APITest.Models;

namespace APITest.Controllers
{
    [Route("api/[controller]")]
    public class PeopleController : Controller
    {
        private readonly TodoContext _context;

        public PeopleController(TodoContext context)
        {
            _context = context;
        }

        // GET: People
        [HttpGet]
        [Produces(typeof(List<Person>))]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Persons.ToListAsync());
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

            var person = await _context.Persons
                .FirstOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            return Ok(person);
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

            await _context.Persons.AddAsync(person);
            return CreatedAtAction(nameof(Create), person);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut("{id}")]
        [Produces(typeof(Person))]
        public async Task<IActionResult> Edit(string id, [FromBody]Person person)
        {
            if (id != person.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(person);
                await _context.SaveChangesAsync();
            }

            return Ok(person);
        }

        // POST: People/Delete/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var person = await _context.Persons.FindAsync(id);
            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
