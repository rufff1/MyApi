using AutoMapper;
using Business.DTOs.Category.Request;
using Business.DTOs.Category.Response;
using Business.DTOs.Product.Request;
using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.MappingProfiles
{
    public class CategoryMappingProfile :Profile
    {

        public CategoryMappingProfile() :base() 
        {
            


            CreateMap<CategoryCreateDTO, Category>().ReverseMap();
            CreateMap<Category, CategoryDTO>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products)).ReverseMap(); 
            CreateMap<CategoryUpdateDTO, Category>().ReverseMap();
        }

    }
}
