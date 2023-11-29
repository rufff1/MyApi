
using Business.DTOs.Common;
using Business.DTOs.Tag.Request;
using Business.DTOs.Tag.Response;
using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Abtraction
{
    public interface ITagService
    {
        Task<Response> CreateAsync(TagCreateDTO model);
        Task<Response> UpdateAsync(int id,TagUpdateDTO model);
        Task<Response<TagResponseDTO>> GetAsync(int id);

        Task<Response<List<TagResponseDTO>>> GetAllAsync();

        Task<Response> DeleteAsync(int id);
    }
}
