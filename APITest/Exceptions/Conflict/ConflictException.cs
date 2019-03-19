using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APITest.Exceptions.Conflict
{
    public class ConflictException : Exception
    {
        public string Entity;
        public string Id;

        public override string Message =>
            $"A conflict occurred while trying to create {Entity} with {Id}. Please try again.";

        public ConflictException(string entity, string id)
        {
            Entity = entity;
            Id = id;
        }
    }
}
