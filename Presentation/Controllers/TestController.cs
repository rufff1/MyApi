using Business.DTOs.Common;
using Business.DTOs.Team.Request;
using Business.DTOs.Test;
using Business.Services.Abtraction;
using Business.Services.Concered;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        public readonly ITestService _testService;


        public TestController(ITestService testService)
        {

            _testService = testService;
        }

        #region Documentation
        /// <summary>
        /// test yaradılması üçün
        /// </summary>
        /// <param name="model"></param>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response))]
        #endregion
        [HttpPost("Create")]
        public async Task<Response> CreateAsync(TestCreateDTO model)
        {
           return await _testService.CreateAsync(model);  
        }
    }
}
