using AutoMapper;
using Business.DTOs.TeamPosition.Request;
using Business.DTOs.TeamPosition.Response;
using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.MappingProfiles
{
    public class TeamPositionMappingProfile :Profile
    {
        public TeamPositionMappingProfile()
        {
                CreateMap<TeamPositionCreateDTO , TeamPosition>().ReverseMap();
                CreateMap<TeamPositionUpdateDTO , TeamPosition>().ReverseMap();
                CreateMap<TeamPosition, TeamPositionReponseDTO >().ReverseMap();
        }
    }
}
