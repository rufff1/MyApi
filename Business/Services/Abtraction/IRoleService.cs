using AutoMapper;
using Business.DTOs.Common;
using Business.DTOs.Role.Request;
using Business.DTOs.Role.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Abstract
{
    public interface IRoleService 
    {
        Task<Response<List<RoleDTO>>> GetAllAsync();

        Task<Response> CreateAsync(RoleCreateDTO model);

    }
}
