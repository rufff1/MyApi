using AutoMapper;
using Business.DTOs.Role.Request;
using Business.DTOs.Role.Response;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.MappingProfiles
{
    public class RoleMappingProfile : Profile
    {
        public RoleMappingProfile() : base()
        {
            CreateMap<IdentityRole, RoleDTO>();
            CreateMap<RoleCreateDTO, IdentityRole>();

        }
    }
}
