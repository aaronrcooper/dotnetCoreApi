using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace APITest.Domain.Models
{
    public class TodoItem
    {
        // Generate a GUID for this field
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }
        [ForeignKey("User")]
        public string userId { get; set; }

        public virtual User user { get; set; }
    }
}
