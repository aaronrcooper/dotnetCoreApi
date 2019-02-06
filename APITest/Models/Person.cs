using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace APITest.Models
{
    public class Person
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public string Email { get; set; }
        public User User { get; set; }
    }
}
