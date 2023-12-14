using Business.DTOs.Blog.Request;
using Business.DTOs.Blog.Response;
using Business.DTOs.Common;

using Business.Services.Abtraction;
using Business.Services.Concered;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }


        #region Documentation
        /// <summary>
        /// blog siyahısını götürmək üçün
        /// </summary>
        /// <remarks>
        /// <ul>
        ///  <li><b>Type:</b> 0 - Personal, 1 - Business, 2 - News, 3 - Professional</li>
        /// </ul>
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<List<BlogResponseDTO>>))]
        #endregion


        [HttpGet()]
      
        public async Task<Response<List<BlogGetCategoryTag>>> GetAllAsync()
        {
            return await _blogService.GetAllAsync();
        }



        #region Documentation
        /// <summary>
        ///  blog id parametrinə görə götürülməsi üçün
        /// </summary>
        /// <param name="id"></param>
        ///   /// <remarks>
        /// <ul>
        ///  <li><b>Type:</b> 0 - Personal, 1 - Business, 2 - News, 3 - Professional</li>
        /// </ul>
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<BlogResponseDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Response))]
        #endregion
        [HttpGet("GetById")]
        
        public async Task<Response<BlogResponseDTO>> GetAsync(int id)
        {
            return await _blogService.GetAsync(id);
        }

        #region Documentation
        /// <summary>
        /// blog yaradılması üçün
        /// </summary>
        /// <param name="model"></param>
        ///   /// <remarks>
        /// <ul>
        ///  <li><b>Type:</b> 0 - Personal, 1 - Business, 2 - News, 3 - Professional</li>
        /// </ul>
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response))]
        #endregion
        [HttpPost("Create")]
        public async Task<Response> CreateAsync([FromForm] BlogCreateDTO model)
        {
            return await _blogService.CreateAsync(model);
        }

        #region Documentation
        /// <summary>
        /// blog redaktə olunması üçün
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        ///   /// <remarks>
        /// <ul>
        ///  <li><b>Type:</b> 0 - Personal, 1 - Business, 2 - News, 3 - Professional</li>
        /// </ul>
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Response))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response))]
        #endregion
        [HttpPut("Update")]
        public async Task<Response> UpdateAsync(int id, [FromForm] BlogUpdateDTO model)
        {
            return await _blogService.UpdateAsync(id, model);
        }

        #region Documentation
        /// <summary>
        ///  blog silinməsi
        /// </summary>
        /// <param name="id"></param>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Response))]
        #endregion
        [HttpDelete("Delete")]
        public async Task<Response> DeleteAsync(int id)
        {
            return await _blogService.DeleteAsync(id);
        }

    }
}
