using AutoMapper;
using Business.DTOs.Auth.Request;
using Business.DTOs.Auth.Response;
using Business.DTOs.Common;
using Business.Exceptions;
using Business.Services.Abtraction;
using Business.Validators.Auth;
using Common.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Concered
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;
        public AuthService(UserManager<User> userManager,
                           RoleManager<IdentityRole> roleManager,
                           IMapper mapper,IConfiguration configuration,ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _configuration = configuration;
           _logger = logger;
        }

        public async Task<Response<AuthLoginResponseDTO>> LoginAsync(AuthLoginDTO model)
        {
            var result = await new AuthLoginDTOValidator().ValidateAsync(model);
            if (!result.IsValid)
            {
                _logger.LogError("model validator error");
                throw new ValidationException(result.Errors);
            }                                     
            var user = await _userManager.FindByNameAsync(model.Email);

            if (user is null)
            {
                _logger.LogError("Poçt ünvanı və ya şifrə yalnışdır");
                throw new UnauthorizedException("Poçt ünvanı və ya şifrə yalnışdır");
            }

            var isSucceededLoginCheck = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!isSucceededLoginCheck)
            {
                _logger.LogError("Poçt ünvanı və ya şifrə yalnışdır");
                throw new UnauthorizedException("Poçt ünvanı və ya şifrə yalnışdır");
            }


            //---------- Create Token ----------


            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role , user.Role),
             
            };

            //var roles = await _userManager.GetRolesAsync(user);

            //foreach (var role in roles)
            //{
            //    claims.Add(new Claim(ClaimTypes.Role, role));
            //}

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.Now.AddHours(3),
                claims: claims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );


            _logger.LogInformation("ugurla login olundu");
            return new Response<AuthLoginResponseDTO>
            {
                Data = new AuthLoginResponseDTO
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token)
                },
                Message = "ugurla login olundu"
            };
        }



       


        
        public async Task<Response> RegisterAsync(AuthRegisterDTO model)
        {

            var result = await new AuthRegisterDTOValidator().ValidateAsync(model);
            if (!result.IsValid)
            {
                _logger.LogError("model validator error");
                throw new ValidationException(result.Errors);
            }

            var user = await _userManager.FindByNameAsync(model.Email);
            if (user is not null)
            {
                _logger.LogError($"{model.Email} bu email artig movcuddur");
                throw new ValidationException("Bu istifadəçi artıq mövcuddur");
            }

                

            user = _mapper.Map<User>(model);

            user.Role = "User";


            var registerResult = await _userManager.CreateAsync(user, model.Password);

            if (!registerResult.Succeeded)
            {
                _logger.LogError($"Failed to register {model.Email}");
                throw new ValidationException(registerResult.Errors);
            }
          
            var role = await _roleManager.FindByNameAsync("User");
            if (role is null)
            {
                _logger.LogError("Rol tapılmadı");
                throw new NotFoundException("Rol tapılmadı");
            }

            user.Role = "User";

            var roleResult = await _userManager.AddToRoleAsync(user, role.Name ?? string.Empty);
            if (!roleResult.Succeeded)
            {
                _logger.LogError($"{role.Name} not found");
                throw new ValidationException(roleResult.Errors);
            }

            _logger.LogInformation("İstifadəçi uğurla yaradıldı");
            return new Response
            {
                Message = "İstifadəçi uğurla yaradıldı"
            };
          
        }
    }
}
