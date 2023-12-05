using AutoMapper;
using Business.DTOs.TeamPosition.Request;
using Business.DTOs.Test;
using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.MappingProfiles
{
     public class TestMappingProfile :Profile
    {
        public TestMappingProfile() :base()
        {
            CreateMap<TestCreateDTO, Test>().ReverseMap();
        }
    }
}
