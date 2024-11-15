﻿using AutoMapper;
using Udemy.Domain.Models;
using Udemy.Presentation.DTOs;

namespace Udemy.Presentation.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<CourseDTO,Course>().ReverseMap().
                ForMember(m=>m.CategoryName,
                o=>o.MapFrom(c=>c.Category.Category));

            CreateMap<CourseCreationDTO ,Course>();
        }
    }
}
