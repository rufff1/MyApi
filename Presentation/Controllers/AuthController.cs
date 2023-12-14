using Business.DTOs.Auth.Request;
using Business.DTOs.Auth.Response;
using Business.DTOs.Common;
using Business.Services.Abtraction;
using Microsoft.AspNetCore.Mvc;


namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        #region Documentation
        /// <summary>
        /// İstifadəçinin qeydiyyatdan keçməsi üçün
        /// </summary>
        /// <param name="model"></param>
        #endregion
        [HttpPost("register")]
        public async Task<Response> RegisterAsync([FromBody]AuthRegisterDTO model)
        {
            return await _authService.RegisterAsync(model);

           
        }

        #region Documentation
        /// <summary>
        /// İstifadəçilərin daxil olması üçün
        /// </summary>
        /// <param name="model"></param>
        #endregion
        [HttpPost("login")]
        public async Task<Response<AuthLoginResponseDTO>>LoginAsync(AuthLoginDTO model)
        {

            return await _authService.LoginAsync(model);
        }

        #region Documentation
        /// <summary>
        /// İstifadəçilərin sifre deyismesi üçün
        /// </summary>
        /// <param name="model"></param>
        #endregion
        [HttpPost("ChangePaswoord")]
        public async Task<Response> ChangePaswoordAsync(ChangePaswoordDTO model)
        {

            return await _authService.ChangePaswoordAsync(model);
        }



        //[HttpPost("CreateAdmin")]
        //public async Task<Response> RegistrationAdminAsync(AuthRegisterDTO model)
        //{

        //    return await _authService.RegistrationAdminAsnyc(model);
        //}



    }
}
