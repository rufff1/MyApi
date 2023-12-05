using AutoMapper;
using Business.DTOs.Country.Request;
using Business.DTOs.Country.Response;
using Common.Constants.Country;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.MappingProfiles
{
    public class CountryMappingProfile : Profile
    {
        public CountryMappingProfile() :base()
        {
            CreateMap<Country , CountryCreateDTO>().ReverseMap();
            CreateMap<Country , CountryUpdateDTO>().ReverseMap();   
            CreateMap<CountryResponseDTO , Country>().ReverseMap(); 
        }
    }
}
