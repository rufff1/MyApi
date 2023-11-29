using AutoMapper;
using Business.DTOs.Auth.Request;
using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.MappingProfiles
{
    public class AuthMappingProfile : Profile
    {
        public AuthMappingProfile() :base()
        {
            CreateMap<AuthRegisterDTO, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
        }
    }
}
