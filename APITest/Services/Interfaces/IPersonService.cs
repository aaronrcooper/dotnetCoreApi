using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTO;


namespace APITest.Services
{
    public interface IPersonService
    {
        Task<Person> Get(string id);
        Task<IEnumerable<Person>> GetAll();
        Task<Person> Update(string id, Person person);
        void Delete(string id);
        Task<Person> Create(Person person);
    }
}
