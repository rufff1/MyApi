
using Business.DTOs.Category.Request;
using Business.DTOs.Category.Response;
using Business.DTOs.Common;
using Business.DTOs.Product.Request;
using Business.Services.Abtraction;
using Common.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        #region Documentation
        /// <summary>
        /// Category siyahısını götürmək üçün
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<List<CategoryDTO>>))]
       

        #endregion
        [HttpGet()]
        public async Task<Response<List<CategoryDTO>>> GetAllAsync()
        {
            return await _categoryService.GetAllAsync();
        }

        #region Documentation
        /// <summary>
        ///  Category id parametrinə görə götürülməsi üçün
        /// </summary>
        /// <param name="id"></param>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<CategoryDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Response))]
        #endregion
        [HttpGet("GetById")]
        public async Task<Response<CategoryDTO>> GetAsync(int id)
        {
            return await _categoryService.GetAsync(id);
        }

        #region Documentation
        /// <summary>
        /// category yaradılması üçün
        /// </summary>
        /// <param name="model"></param>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response))]
        #endregion
        [HttpPost("Create")]
        public async Task<Response> CreateAsync([FromForm]CategoryCreateDTO model)
        {
            return await _categoryService.CreateAsync(model);
        }

        #region Documentation
        /// <summary>
        /// category redaktə olunması üçün
        /// </summary>
        /// <param name="model"></param>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Response))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response))]
        #endregion
        [HttpPut("Update")]
        public async Task<Response> UpdateAsync([FromForm] CategoryUpdateDTO model)
        {
            return await _categoryService.UpdateAsync(model);
        }

        #region Documentation
        /// <summary>
        ///  Category silinməsi
        /// </summary>
        /// <param name="id"></param>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Response))]
        #endregion
        [HttpDelete("Delete")]
        public async Task<Response> DeleteAsync(int id)
        {
            return await _categoryService.DeleteAsync(id);
        }
    }
}
