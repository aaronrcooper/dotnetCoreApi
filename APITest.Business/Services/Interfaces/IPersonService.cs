using APITest.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Business.Services
{
    public interface IPersonService
    {
        Task<Person> Get(Guid id);
        Task<IEnumerable<Person>> GetAll();
        Task<Person> Update(Guid id, Person person);
        void Delete(Guid id);
        Task<Person> Create(Person person);
    }
}
