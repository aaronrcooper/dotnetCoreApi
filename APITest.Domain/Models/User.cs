using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace APITest.Domain.Models
{
    public class User
    {
        // Generate a GUID for this field 
        [ForeignKey(nameof(Person))]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(32)]
        public string Username { get; set; }
        [Required]
        [MaxLength(16)]
        [MinLength(16)]
        public byte[] Salt { get; set; }
        [Required]
        public string HashedPassword { get; set; }
        [ForeignKey(nameof(Role))]
        public Guid RoleId { get; set; }

        // Navigation Properties
        public virtual Person Person { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<TodoItem> TodoItems { get; set; }
    }

    public class UserPost
    {
        public Person Person { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }

    public class UserCredentials
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class UserPut
    {
        public Person Person { get; set; }
        public Guid UserId { get; set; }
        public string Password { get; set; }
    }

    public class LoggedInUser
    {
        public User User { get; set; }
        public string JSONWebToken { get; set; }
    }
}
