using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APITest.Shared;
using Microsoft.EntityFrameworkCore;

namespace APITest.Models
{
    public class TodoContext : DbContext
    {
        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public TodoContext(DbContextOptions<TodoContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Person>(p =>
            {
                p.ToTable("People");
                p.HasIndex(person => person.Email).IsUnique();
                p.HasOne(person => person.User).WithOne(u => u.Person).OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<User>(c =>
            {
                c.HasIndex(e => e.Username).IsUnique();
                c.HasOne(u => u.Person).WithOne(p => p.User).OnDelete(DeleteBehavior.Cascade);
            });


            #region *** SEED DATA ***

            // Admin Guid
            Role adminRole = new Role() { Id = Guid.NewGuid().ToString(), UserRole = "Administrator" };
            builder.Entity<Role>().HasData(adminRole);
            builder.Entity<Role>().HasData(new Role() { Id = Guid.NewGuid().ToString(), UserRole = "User" });
            // Initialize admin Guid
            string adminGuid = Guid.NewGuid().ToString();

            builder.Entity<Person>().HasData(new Person()
            {
                Address = "N/a",
                City = "Pittsburgh",
                Email = "N/a",
                FirstName = "Aaron",
                LastName = "Cooper",
                Id = adminGuid,
                State = "PA",
                Zipcode = "N/a"
            });

            //Add admin user
            var hashedPassword = Auth.GeneratePassword("password");
            builder.Entity<User>().HasData(new User() { HashedPassword = hashedPassword.HashedPassword, Id = adminGuid, Salt = hashedPassword.Salt, Username = "Admin", RoleId = adminRole.Id });

            #endregion
        }
    }
}
