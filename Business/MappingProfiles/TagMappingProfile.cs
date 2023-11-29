using AutoMapper;
using Business.DTOs.Tag.Request;
using Business.DTOs.Tag.Response;
using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.MappingProfiles
{
    public class TagMappingProfile :Profile
    {
        public TagMappingProfile() :base()
        {

            CreateMap<TagCreateDTO , Tag>().ReverseMap();
            CreateMap<TagUpdateDTO , Tag>().ReverseMap();
            CreateMap<Tag, TagResponseDTO >().ReverseMap();


        }
    }
}
