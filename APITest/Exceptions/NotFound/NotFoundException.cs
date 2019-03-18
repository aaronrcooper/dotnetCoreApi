using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APITest.Exceptions
{
    public class NotFoundException : Exception
    {
        public string Entity { get; set; }
        public string Field { get; set; }
        public string Id { get; set; }
        public override string Message => $"Unable to find {Entity} with {Field} value of {Id}";

        protected NotFoundException(string entity, string field, string id)
        {
            Entity = entity;
            Field = field;
            Id = id;
        }
    }
}
