using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Exceptions.NotFound;
using Microsoft.EntityFrameworkCore;
using APITest.Domain.Models;
using APITest.Domain;

namespace Business.Services
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

        public void Delete(Guid id)
        {
            var person = _context.Persons.FirstOrDefault(p => p.Id == id);

            if (person == null)
            {
                throw new PersonNotFoundException(id.ToString());
            }

            _context.Persons.Remove(person);
            _context.SaveChanges();
        }

        public async Task<Person> Get(Guid id)
        {
            var person = await _context.Persons.FirstOrDefaultAsync(p => p.Id == id);

            if (person == null)
            {
                throw new PersonNotFoundException(id.ToString());
            }

            return person;
        }

        public async Task<IEnumerable<Person>> GetAll()
        {
            return await _context.Persons.ToListAsync();
        }

        public async Task<Person> Update(Guid id, Person person)
        {
            if (id == person.Id)
            {
                throw new PersonNotFoundException(id.ToString());
            }
            _context.Entry(person).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return person;

        }
    }
}
