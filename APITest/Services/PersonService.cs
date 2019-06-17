using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APITest.Exceptions.NotFound;
using Microsoft.EntityFrameworkCore;
using DTO;
using APITest.Models;

namespace APITest.Services
{
    public class PersonService : IPersonService
    {
        private readonly TodoContext _context;
        public PersonService(TodoContext context)
        {
            _context = context;
        }

        public async Task<Person> Create(Person person)
        {
            await _context.Persons.AddAsync(person);
            return person;
        }

        public void Delete(string id)
        {
            var person = _context.Persons.FirstOrDefault(p => p.Id == id);

            if (person == null)
            {
                throw new PersonNotFoundException(id);
            }

            _context.Persons.Remove(person);
            _context.SaveChanges();
        }

        public async Task<Person> Get(string id)
        {
            var person = await _context.Persons.FirstOrDefaultAsync(p => p.Id == id);

            if (person == null)
            {
                throw new PersonNotFoundException(id);
            }

            return person;
        }

        public async Task<IEnumerable<Person>> GetAll()
        {
            return await _context.Persons.ToListAsync();
        }

        public async Task<Person> Update(string id, Person person)
        {
            if (id == person.Id)
            {
                throw new PersonNotFoundException(id);
            }
            _context.Entry(person).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return person;

        }
    }
}
