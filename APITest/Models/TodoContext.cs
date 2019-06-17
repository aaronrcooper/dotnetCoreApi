using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APITest.Shared;
using Microsoft.EntityFrameworkCore;
using DTO;

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

            builder.Entity<User>(c => { c.HasMany(u => u.TodoItems).WithOne(t => t.user).OnDelete(DeleteBehavior.Cascade); });


            #region *** SEED DATA ***

            // Admin Guid
            Role adminRole = new Role() { Id = "a8cc16b7-aa6b-47d3-909a-06fdcae81619", UserRole = "Administrator" };
            builder.Entity<Role>().HasData(adminRole);
            builder.Entity<Role>().HasData(new Role() { Id = "8110bff0-12a7-4cc7-906d-a9c052727e06", UserRole = "User" });

            builder.Entity<Person>().HasData(new Person()
            {
                Id = "eaa559e4-090c-4706-a776-ffa16e7a2191",
                Address = "N/a",
                City = "Pittsburgh",
                Email = "N/a",
                FirstName = "Aaron",
                LastName = "Cooper",
                State = "PA",
                Zipcode = "N/a"
            });

            //Add admin user
            var hashedPassword = Auth.GeneratePassword("password");
            builder.Entity<User>().HasData(new User() { HashedPassword = "Ov4B87zmh9j/dEG/y/BQlT3S8FA=", Id = "eaa559e4-090c-4706-a776-ffa16e7a2191",
                Salt = new byte[] { 92, 251, 117, 81, 232, 198, 132, 52, 28, 94, 233, 112, 135, 156, 117, 187 }, Username = "Admin", RoleId = adminRole.Id });
            #endregion
        }
    }
}
