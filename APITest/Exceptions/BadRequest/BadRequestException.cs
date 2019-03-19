using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APITest.Exceptions.BadRequest
{
    public class BadRequestException : Exception
    {
        private string Entity;
        private string Id;

        public override string Message => $"A(n) {Entity} with {Id} was unable to be created";

        public BadRequestException(string entity, string id)
        {
            Entity = entity;
            Id = id;
        }
    }
}
