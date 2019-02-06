using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace APITest.Models
{
    public class User
    {
        // Generate a GUID for this field 
        [Key]
        [ForeignKey("Person")]
        public string Id { get; set; }
        public Person Person { get; set; }
        public string Username { get; set; }
        public string Hash { get; set; }
        public string HashedPassword { get; set; }
    }
}
