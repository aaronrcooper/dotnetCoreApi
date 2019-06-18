using APITest.Domain.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace APITest.Business.Mappers
{
    public class TodoProfile : Profile
    {
        public TodoProfile()
        {
            CreateMap<TodoItem, DTO.TodoItem>();
        }
    }
}
