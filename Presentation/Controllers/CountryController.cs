using Business.DTOs.Common;
using Business.DTOs.Country.Request;
using Business.DTOs.Country.Response;
using Business.Services.Abtraction;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class CountryController : ControllerBase
    {
        public readonly ICountryService _countryService;

        public CountryController(ICountryService countryService)
        {
           _countryService = countryService;
        }


        #region Documentation
        /// <summary>
        ///  Country id parametrinə görə götürülməsi üçün
        /// </summary>
        /// <param name="id"></param>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<CountryResponseDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Response))]
        #endregion
        [HttpGet("GetById")]
        public async Task<Response<CountryResponseDTO>> GetAsync(int id)
        {

              return await _countryService.GetAsync(id);    
        }

        #region Documentation
        /// <summary>
        /// Country siyahısını götürmək üçün
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<List<CountryResponseDTO>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Response))]
        #endregion
        [HttpGet("GetAll")]
        public async Task<Response<List<CountryResponseDTO>>> GetAllAsync()
        {
           return await _countryService.GetAllAsync();
        }

        #region Documentation
        /// <summary>
        /// Country redaktə olunması üçün
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Response))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response))]
        #endregion
        [HttpPut("Update")]
        public async Task<Response> UpdateAyns(int id, CountryUpdateDTO model)
        {
            return await _countryService.UpdateAsync(id,model);
           
        }


        #region Documentation
        /// <summary>
        ///  Country silinməsi
        /// </summary>
        /// <param name="id"></param>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Response))]
        #endregion
        [HttpDelete("Delete")]
        public async Task<Response> DeleteAsync(int id)
        {
            return await _countryService.DeleteAsync(id);
        }




        #region Documentation
        /// <summary>
        /// Country yaradılması üçün
        /// </summary>
        /// <param name="model"></param>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response))]
        #endregion
        [HttpPost("Create")]
        public async Task<Response> CreateAsync(CountryCreateDTO model)
        {
            return await _countryService.CreateAsync(model);
        }



    }
}
