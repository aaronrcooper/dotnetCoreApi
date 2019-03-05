using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using APITest.Shared;

namespace APITest.Models
{
    public class User
    {
        // Generate a GUID for this field 
        [ForeignKey(nameof(Person))]
        public string Id { get; set; }
        public virtual Person Person { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        [MaxLength(16)]
        [MinLength(16)]
        public byte[] Salt { get; set; }
        [Required]
        [StringLength(20)]
        public string HashedPassword { get; set; }
    }

    public class UserPost
    {
        public Person Person { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class UserCredentials
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
