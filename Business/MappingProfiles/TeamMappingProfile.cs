using AutoMapper;
using Business.DTOs.Team.Request;
using Business.DTOs.Team.Response;
using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.MappingProfiles
{
    public class TeamMappingProfile : Profile
    {
        public TeamMappingProfile() :base()
        {
            CreateMap<TeamCreateDTO , Team>().ReverseMap();
            CreateMap<TeamUpdateDTO , Team>().ReverseMap();
            CreateMap<Team , TeamResponseDTO>().ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country)).ReverseMap();
        }
    }
}
