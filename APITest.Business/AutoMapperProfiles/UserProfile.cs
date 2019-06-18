using APITest.Domain.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace APITest.Business.AutoMapperProfiles
{
    class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, DTO.User>();
            CreateMap<LoggedInUser, DTO.LoggedInUser>()
                .ForMember(p => p.Username, opts => opts.MapFrom(source => source.User.Username));
        }
    }
}
