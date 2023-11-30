using Business.DTOs.Common;
using Business.DTOs.Team.Request;
using Business.DTOs.Team.Response;
using Business.DTOs.TeamPosition.Response;
using Business.Services.Abtraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService _teamService;

        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }



        #region Documentation
        /// <summary>
        ///  team id parametrinə görə götürülməsi üçün
        /// </summary>
        /// <param name="id"></param>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<TeamPositionReponseDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Response))]
        #endregion
        [HttpGet("GetById")]
        public async Task<Response<TeamResponseDTO>> GetAsync(int id)
        {
            return await _teamService.GetAsync(id); 
        }

        #region Documentation
        /// <summary>
        /// team siyahısını götürmək üçün
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<List<TeamPositionReponseDTO>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Response))]
        #endregion
        [HttpGet("GetAll")]
        public async Task<Response<List<TeamResponseDTO>>> GetAllAsync()
        {
            return await _teamService.GetAllAsync();
        }


        #region Documentation
        /// <summary>
        /// team yaradılması üçün
        /// </summary>
        /// <param name="model"></param>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response))]
        #endregion
        [HttpPost("Create")]
        public async Task<Response> CreateAsync([FromForm] TeamCreateDTO model)
        {
            return await _teamService.CreateAsync(model);
        }

        #region Documentation
        /// <summary>
        /// team redaktə olunması üçün
        /// </summary>
        /// <param name="model"></param>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Response))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response))]
        #endregion
        [HttpPut("Update")]
        public async Task<Response> UpdateAsync([FromForm] TeamUpdateDTO model)
        {
            return await _teamService.UpdateAsync(model);
        }



        #region Documentation
        /// <summary>
        ///  team silinməsi  
        /// </summary>
        /// <param name="id"></param>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Response))]
        #endregion
        [HttpDelete("Delete")]
        public async Task<Response> DeleteAsync(int id)
        {
            return await _teamService.DeleteAsync(id);
        }




    }
}
