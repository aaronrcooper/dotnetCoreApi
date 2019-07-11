using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Business.Exceptions.NotFound;
using Business.Services;
using AutoMapper;
using APITest.Domain.Models;

namespace APITest.Controllers
{
    [Route("api/[controller]")]
    public class PeopleController : Controller
    {
        private IPersonService _personService;
        private IMapper _mapper;

        public PeopleController(IPersonService personService, IMapper mapper)
        {
            _personService = personService;
            _mapper = mapper;
        }

        // GET: People
        [HttpGet]
        [Produces(typeof(List<DTO.Person>))]
        public async Task<IActionResult> GetPerson()
        {
            var people = await _personService.GetAll();

            return Ok(_mapper.Map<IEnumerable<Person>, IEnumerable<DTO.Person>>(people));
        }

        // GET: People/Details/5
        [HttpGet("{id}")]
        [Produces(typeof(DTO.Person))]
        public async Task<IActionResult> PersonDetails(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var person = await _personService.Get(id);
                _mapper.Map<Person, DTO.Person>(person);
                return Ok();
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
        [Produces(typeof(DTO.Person))]
        public async Task<IActionResult> CreatePerson([FromBody] Person person)
        {
            if (ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                await _personService.Create(person);
                return CreatedAtAction(nameof(CreatePerson), _mapper.Map<Person, DTO.Person>(person));
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
        [Produces(typeof(DTO.Person))]
        public async Task<IActionResult> EditPerson(Guid id, [FromBody]Person person)
        {
            try
            {
                await _personService.Update(id, person);
            }
            catch (PersonNotFoundException e)
            {
                return NotFound(e.Message);
            }

            return Ok(_mapper.Map<Person, DTO.Person>(person));
        }

        // POST: People/Delete/5
        [HttpDelete("{id}")]
        public IActionResult DeletePerson(Guid id)
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
