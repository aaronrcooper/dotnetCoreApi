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
        [Required]
        public string Username { get; set; }
        [Required]
        public byte[] Salt { get; set; }
        [Required]
        public string HashedPassword { get; set; }
    }

    public class UserPost
    {
        public Person Person { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
