using Business.DTOs.Common;
using Business.DTOs.Team.Request;
using Business.DTOs.Team.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Abtraction
{
    public interface ITeamService
    {

        Task<Response<TeamResponseDTO>> GetAsync(int id);
        Task<Response<List<TeamResponseDTO>>> GetAllAsync();
        Task<Response> CreateAsync(TeamCreateDTO model);
        Task<Response> UpdateAsync(int id,TeamUpdateDTO model);
        Task<Response> DeleteAsync(int id);


    }
}
