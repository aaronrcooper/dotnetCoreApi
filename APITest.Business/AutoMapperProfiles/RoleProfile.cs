using APITest.Domain.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace APITest.Business.AutoMapperProfiles
{
    class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<Role, DTO.Role>();
        }
    }
}
