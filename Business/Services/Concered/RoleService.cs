using AutoMapper;
using Business.DTOs.Common;
using Business.DTOs.Role.Request;
using Business.DTOs.Role.Response;
using Business.Services.Abstract;
using Business.Validators.Role;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace Business.Services.Concered
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        

       
        public RoleService(RoleManager<IdentityRole> roleManager,
                          IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
            
        }

        public async Task<Response<List<RoleDTO>>> GetAllAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            if (roles == null)
            {
                return new Response<List<RoleDTO>>
                {
                    Message = "hec bir role yoxdur"
                };
            }
            return new Response<List<RoleDTO>>
            {
                Data = _mapper.Map<List<RoleDTO>>(roles),
                Message= "Rollarin siyahisi elde edildi"
            };
        }

        public async Task<Response> CreateAsync(RoleCreateDTO model)
        {
            var result = await new RoleDTOValidator().ValidateAsync(model);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);

            var role = await _roleManager.FindByNameAsync(model.Name);
            if (role is not null)
                throw new ValidationException("Bu adda rol artıq mövcuddur");

            var roleResult = await _roleManager.CreateAsync(_mapper.Map<IdentityRole>(model));
        

            return new Response
            {
                Message = "Rol uğurla yaradıldı"
            };
        }

    }
}
