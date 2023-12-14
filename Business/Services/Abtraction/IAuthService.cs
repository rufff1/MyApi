using Business.DTOs.Auth.Request;
using Business.DTOs.Auth.Response;
using Business.DTOs.Common;
using Common.Entities;

namespace Business.Services.Abtraction
{
    public interface IAuthService
    {
        Task<Response> RegisterAsync(AuthRegisterDTO model);
        Task<Response<AuthLoginResponseDTO>> LoginAsync(AuthLoginDTO model);
        Task<Response> ChangePaswoordAsync(ChangePaswoordDTO model);
        //Task<Response> RegistrationAdminAsnyc(AuthRegisterDTO model);

    }
}
