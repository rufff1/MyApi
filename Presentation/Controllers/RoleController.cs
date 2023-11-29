using Business.DTOs.Common;
using Business.DTOs.Role.Request;
using Business.DTOs.Role.Response;
using Business.Services.Abstract;
using Business.Validators.Role;
using DataAccess.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
       private readonly IRoleService _roleService;
       
      
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
       
        }

       
        #region Documentation
        /// <summary>
        /// Rolların siyahısı
        /// </summary>
        #endregion
        [HttpGet]
        public async Task<Response<List<RoleDTO>>> GetAllAsync()
        {
            return await _roleService.GetAllAsync();
        }

        #region Documentation
        /// <summary>
        ///  Rol yaratmaq üçün
        /// </summary>
        /// <param name="model"></param>
        #endregion
        [HttpPost]
        public async Task<Response> CreateAsync(RoleCreateDTO model)
        {
            return await _roleService.CreateAsync(model);
        }
    }
}
