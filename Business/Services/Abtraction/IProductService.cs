
using Business.DTOs.Common;
using Business.DTOs.Product.Request;
using Business.DTOs.Product.Response;
using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Abtraction
{
    public interface IProductService
    {
        Task<Response> CreateAsync(ProductCreateDTO model);
        Task<Response> UpdateAsync(int id,ProductUpdateDTO model);
        Task<Response<List<ProductGetTags>>> GetAllAsync();
        Task<Response<ProductResponseDTO>> GetAsync(int id);
        Task<Response> DeleteAsync(int id);

    }
}
