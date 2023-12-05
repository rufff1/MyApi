using AutoMapper;
using Business.DTOs.Blog.Request;
using Business.DTOs.Blog.Response;
using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.MappingProfiles
{
    public class BlogMappingProfile :Profile
    {
        public BlogMappingProfile()
        { 
               CreateMap<BlogCreateDTO, Blog>().ReverseMap();
               CreateMap<BlogUpdateDTO, Blog>().ReverseMap();


               CreateMap<Blog, BlogResponseDTO>().ReverseMap();


            CreateMap<Blog, BlogGetCategoryTag>().ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.BlogTags.Select(pt => pt.Tag)))
                 .ReverseMap();
          



        }
    }
}
