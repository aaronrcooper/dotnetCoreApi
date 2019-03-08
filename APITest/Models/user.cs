using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using APITest.Shared;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace APITest.Models
{
    public class User
    {
        // Generate a GUID for this field 
        [ForeignKey(nameof(Person))]
        public string Id { get; set; }
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
        public string RoleId { get; set; }

        // Navigation Properties
        public virtual Person Person { get; set; }
        public virtual Role Role { get; set; }

        public static async Task<bool> isAdmin(TodoContext context, User User)
        {
            if (context == null || User == null)
            {
                return false;
            }

            var user = await context.Users.FirstOrDefaultAsync(u => u.RoleId == User.RoleId);

            if (user == null)
            {
                return false;
            }

            await context.Entry(user).Reference(u => u.Role).LoadAsync();
            return user.Role.UserRole.ToUpper() == "ADMININSTATOR";
        }
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

    public class UserPut
    {
        public Person Person { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
    }
}
