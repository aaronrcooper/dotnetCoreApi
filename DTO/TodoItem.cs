﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DTO
{
    public class TodoItem
    {
        // Generate a GUID for this field 
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }
        [ForeignKey("User")]
        public string userId { get; set; }

        public virtual User user { get; set; }
    }
}
