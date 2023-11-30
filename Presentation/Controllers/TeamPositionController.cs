using Business.DTOs.Common;
using Business.DTOs.Tag.Response;
using Business.DTOs.TeamPosition.Request;
using Business.DTOs.TeamPosition.Response;
using Business.Services.Abtraction;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamPositionController : ControllerBase
    {
        public readonly ITeamPositionService _teamPositionService;

        public TeamPositionController(ITeamPositionService teamPositionService)
        {
                _teamPositionService = teamPositionService;

        }



        #region Documentation
        /// <summary>
        ///  position id parametrinə görə götürülməsi üçün
        /// </summary>
        /// <param name="id"></param>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<TeamPositionReponseDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Response))]
        #endregion
        [HttpGet("GetById")]
        public async Task<Response<TeamPositionReponseDTO>> GetAsync(int id)
        {
           return await _teamPositionService.GetAsync(id);
        }



        #region Documentation
        /// <summary>
        /// position siyahısını götürmək üçün
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<List<TeamPositionReponseDTO>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Response))]
        #endregion
        [HttpGet("GetAll")]
        public async Task<Response<List<TeamPositionReponseDTO>>> GetAllAsync()
        {
            return await _teamPositionService.GetAllAsync();
        }


        #region Documentation
        /// <summary>
        /// position yaradılması üçün
        /// </summary>
        /// <param name="model"></param>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response))]
        #endregion
        [HttpPost("Create")]
        public async Task<Response> CreateAsync(TeamPositionCreateDTO model)
        {
            return await _teamPositionService.CreateAsync(model);
        }



        #region Documentation
        /// <summary>
        /// position redaktə olunması üçün
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Response))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response))]
        #endregion
        [HttpPut("Update")]
        public async Task<Response> UpdateAsync(int id, TeamPositionUpdateDTO model)
        {
            return await _teamPositionService.UpdateAsync(id, model);
        }


        #region Documentation
        /// <summary>
        ///  position silinməsi
        /// </summary>
        /// <param name="id"></param>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Response))]
        #endregion
        [HttpDelete("Delete")]
        public async Task<Response> DeleteAsnyc(int id)
        {
            return await _teamPositionService.DeleteAsync(id);
        }

    }
}
