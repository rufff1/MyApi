

using Business.DTOs.Common;
using Business.DTOs.Product.Request;
using Business.DTOs.Product.Response;
using Business.Services.Abtraction;
using Business.Services.Concered;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }



        #region Documentation
        /// <summary>
        /// Product siyahısını götürmək üçün
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<List<ProductGetTags>>))]
        #endregion


        [HttpGet()]
        public async Task<Response<List<ProductGetTags>>> GetAllAsync()
        {
            return await _productService.GetAllAsync();
        }



        #region Documentation
        /// <summary>
        ///  Product id parametrinə görə götürülməsi üçün
        /// </summary>
        /// <param name="id"></param>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<ProductResponseDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Response))]
        #endregion
        [HttpGet("GetById")]
        public async Task<Response<ProductResponseDTO>> GetAsync(int id)
        {
            return await _productService.GetAsync(id);
        }

        #region Documentation
        /// <summary>
        /// Product yaradılması üçün
        /// </summary>
        /// <param name="model"></param>
        ///   /// <remarks>
        /// <ul>
        ///  <li><b>Type:</b> 0 - Standart, 1 - Yeni, 2 - Satılmış, 3 - Satışda</li>
        /// </ul>
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response))]
        #endregion
        [HttpPost("Create")]
        public async Task<Response> CreateAsync([FromForm] ProductCreateDTO model)
        {
            return await _productService.CreateAsync(model);
        }

        #region Documentation
        /// <summary>
        /// Product redaktə olunması üçün
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        ///   /// <remarks>
        /// <ul>
        ///  <li><b>Type:</b> 0 - Standart, 1 - Yeni, 2 - Satılmış, 3 - Satışda</li>
        /// </ul>
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Response))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response))]
        #endregion
        [HttpPut("Update")]
        public async Task<Response> UpdateAsync(int id, [FromForm] ProductUpdateDTO model)
        {
            return await _productService.UpdateAsync(id, model);
        }

        #region Documentation
        /// <summary>
        ///  Product silinməsi
        /// </summary>
        /// <param name="id"></param>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Response))]
        #endregion
        [HttpDelete("Delete")]
        public async Task<Response> DeleteAsync(int id)
        {
            return await _productService.DeleteAsync(id);
        }


    }
}
