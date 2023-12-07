using Business.DTOs.Category.Request;
using Business.DTOs.Category.Response;
using Business.DTOs.Common;
using Business.DTOs.Product.Request;
using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Abtraction
{
    public interface ICategoryService
    {
        Task<Response<List<CategoryDTO>>> GetAllAsync();
        Task<Response<CategoryDTO>> GetAsync(int id);
        Task<Response> CreateAsync(CategoryCreateDTO model);
        Task<Response> UpdateAsync(int id, CategoryUpdateDTO model);
        Task<Response> DeleteAsync(int id);

    }
}
