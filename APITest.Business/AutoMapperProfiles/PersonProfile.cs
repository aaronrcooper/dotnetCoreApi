using APITest.Domain.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace APITest.Business.AutoMapperProfiles
{
    class PersonProfile : Profile
    {
        public PersonProfile()
        {
            CreateMap<Person, DTO.Person>();
        }
    }
}
