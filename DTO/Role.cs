﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DTO
{
    public class Role
    {
        public Guid Id { get; set; }

        public string UserRole { get; set; }
    }
}
