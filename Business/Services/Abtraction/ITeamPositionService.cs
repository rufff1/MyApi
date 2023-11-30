
using Business.DTOs.Common;
using Business.DTOs.TeamPosition.Request;
using Business.DTOs.TeamPosition.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Abtraction
{
    public interface ITeamPositionService
    {
        Task<Response> CreateAsync(TeamPositionCreateDTO model);
        Task<Response> UpdateAsync(int id,TeamPositionUpdateDTO model);
        Task<Response<TeamPositionReponseDTO>> GetAsync(int id);
        Task<Response<List<TeamPositionReponseDTO>>> GetAllAsync();
        Task<Response> DeleteAsync(int id);

    }
}
