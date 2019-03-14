using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APITest.Models;

namespace APITest.Services
{
    interface IPersonService
    {
        Person Get(string id);
        IEnumerable<Person> GetAll();
        Person Update(string id, Person person);
        void Delete(string id);
        Person Create(Person person);
    }
}
