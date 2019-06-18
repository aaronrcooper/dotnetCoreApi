using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DTO
{
    public class User
    {
        // Generate a GUID for this field 
        public Guid Id { get; set; }
        public string Username { get; set; }
        public Guid RoleId { get; set; }

        // Navigation Properties
        public virtual Person Person { get; set; }
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
        public string Username { get; set; }
        public string JSONWebToken { get; set; }
    }
}
