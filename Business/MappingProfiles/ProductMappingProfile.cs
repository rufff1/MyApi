using AutoMapper;
using Business.DTOs.Category.Response;
using Business.DTOs.Product.Request;
using Business.DTOs.Product.Response;
using Common.Entities;
using Microsoft.AspNetCore.Routing.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.MappingProfiles
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile() : base()
        {
            CreateMap<Category, ProductFindCategoryDTO>().ReverseMap();
            CreateMap<Product, ProductResponseDTO>().ReverseMap();
            CreateMap<Product, ProductGetTags>()
                 .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.ProductTags.Select(pt => pt.Tag))).ReverseMap();
                 





            CreateMap<ProductCreateDTO, Product>().ReverseMap();
            CreateMap<ProductUpdateDTO, Product>().ReverseMap();
            
        }
    }
}
