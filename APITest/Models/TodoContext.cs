using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace APITest.Models
{
    public class TodoContext : DbContext
    {
        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<User> Users { get; set; }

        public TodoContext(DbContextOptions<TodoContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>(c =>
            {
                c.HasIndex(e => e.Username).IsUnique();
                c.HasOne<Person>(p => p.Person).WithOne(p => p.User).OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
