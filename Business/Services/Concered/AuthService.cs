


using AutoMapper;
using Business.DTOs.Auth.Request;
using Business.DTOs.Auth.Response;
using Business.DTOs.Blog.Response;
using Business.DTOs.Common;
using Business.Exceptions;
using Business.Services.Abtraction;
using Business.Validators.Auth;
using Common.Constants.UserRole;
using Common.Entities;
using FirstMyApi.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http.ModelBinding;

namespace Business.Services.Concered
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;
 
        public AuthService(IConfiguration configuration, UserManager<User> userManager,
                           RoleManager<IdentityRole> roleManager,
                           IMapper mapper,ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _configuration = configuration;
           _logger = logger;
            
        }






        public async Task<Response> ChangePaswoordAsync(ChangePaswoordDTO model)
        {

            var result = await new ChangePaswoordDTOValidator().ValidateAsync(model);
            if (!result.IsValid)
            {
                _logger.LogError("model validator error");
                throw new ValidationException(result.Errors);
            }

            var user = await _userManager.FindByNameAsync(model.Email);

            if (user is null)
            {
                _logger.LogError("Poçt ünvanı yalnışdır");
                throw new UnauthorizedException("Poçt ünvanı  yalnışdır");
            }



           
             

            if (!await _userManager.CheckPasswordAsync(user, model.Password))
            {
                _logger.LogError("şifrə yalnışdır");
                throw new UnauthorizedException(" şifrə yalnışdır");
            }


            var resultpass = await _userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);
            if (!resultpass.Succeeded)
            {
                _logger.LogError("Poçt ünvanı və ya şifrə yalnışdır");
                throw new ValidationException("şifrə deyismedi");
            }

            return new Response
            {
                Message = "Sifre ugurla deyisdirildi"
            };

        }

     
        public string CreateTokenAsync(IEnumerable<Claim> claims)
        {
            


            //var userRoles = await _userManager.GetRolesAsync(user);
            //foreach (var userRole in userRoles)
            //{
            //    await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, userRole));
            //}


            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
           issuer: _configuration["JWT:Issuer"],
           audience: _configuration["JWT:Audience"],
           expires: DateTime.Now.AddDays(1),
           signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
           );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;

        }


        public async Task<Response<AuthLoginResponseDTO>> LoginAsync(AuthLoginDTO model)
        {
          


            var result = await new AuthLoginDTOValidator().ValidateAsync(model);
            if (!result.IsValid)
            {
                _logger.LogError("model validator error");
                throw new ValidationException(result.Errors);
            }
            try
            {
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


            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),


            };

            var userRoles = await _userManager.GetRolesAsync(user);


            foreach (var userRole in userRoles)
            {
                await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, userRole));
                claims.Add(new Claim(ClaimTypes.Role, userRole));
            }


            string token = CreateTokenAsync(claims);
            string refreshToken = GenerateRefreshToken();
            await _userManager.UpdateAsync(user);
            _logger.LogInformation("ugurla login olundu");

            return new Response<AuthLoginResponseDTO>
            {
                Data = new AuthLoginResponseDTO
                {
                    Token = token,
                    RefreshToken = refreshToken
                },
              
                Message = "ugurla login olundu"
            };
            }
            catch (Exception e)
            {

                int a = 32;
            }


            return null;

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


            var registerResult = await _userManager.CreateAsync(user, model.Password);

            if (!registerResult.Succeeded)
            {
                _logger.LogError($"Failed to register {model.Email}");
                throw new ValidationException(registerResult.Errors);
            }

            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            if (await _roleManager.RoleExistsAsync(UserRoles.User))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.User);
            }


            _logger.LogInformation("İstifadəçi uğurla yaradıldı");
            return new Response
            {
                Message = "İstifadəçi uğurla yaradıldı"
            };
          
        }





        public async Task<RefreshModel> GetRefreshToken(RefreshModel model)
        {
           
            var principal = GetPrincipalFromExpiredToken(model.Token);
            string username = principal.Identity.Name;
            var user = await _userManager.FindByNameAsync(username);

            if (user == null || user.RefreshToken != model.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
               
                return model;
            }
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email),

            };

          
            var newToken = CreateTokenAsync(claims);
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);

            model.Token = newToken;
            model.RefreshToken = newRefreshToken;
            return model;
        }




        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTKey:Secret"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }







        //public async Task<Response> RegistrationAdminAsnyc([FromBody] AuthRegisterDTO model)
        //{

        //    var result = await new AuthRegisterDTOValidator().ValidateAsync(model);
        //    if (!result.IsValid)
        //    {
        //        _logger.LogError("model validator error");
        //        throw new ValidationException(result.Errors);
        //    }

        //    var user = await _userManager.FindByNameAsync(model.Email);
        //    if (user is not null)
        //    {
        //        _logger.LogError($"{model.Email} bu email artig movcuddur");
        //        throw new ValidationException("Bu istifadəçi artıq mövcuddur");
        //    }

        //    user = _mapper.Map<User>(model);

        //    var registerResult = await _userManager.CreateAsync(user, model.Password);

        //    if (!registerResult.Succeeded)
        //    {
        //        _logger.LogError($"Failed to register {model.Email}");
        //        throw new ValidationException(registerResult.Errors);
        //    }



        //    if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
        //        await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

        //    if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
        //    {
        //        await _userManager.AddToRoleAsync(user, UserRoles.Admin);
        //    }


        //    _logger.LogInformation("Admin uğurla yaradıldı");
        //    return new Response
        //    {
        //        Message = "Admin uğurla yaradıldı"
        //    };


        //}


    }
}
 