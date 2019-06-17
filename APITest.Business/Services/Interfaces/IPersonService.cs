using APITest.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Business.Services
{
    public interface IPersonService
    {
        Task<DTO.Person> Get(Guid id);
        Task<IEnumerable<DTO.Person>> GetAll();
        Task<DTO.Person> Update(Guid id, Person person);
        void Delete(Guid id);
        Task<DTO.Person> Create(Person person);
    }
}
