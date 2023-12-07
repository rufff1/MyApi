
using Business.DTOs.Common;
using Business.DTOs.Country.Request;
using Business.DTOs.Country.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Abtraction
{
    public interface ICountryService
    {
        Task<Response> CreateAsync(CountryCreateDTO model);
        Task<Response> UpdateAsync(int id,CountryUpdateDTO model);
        Task<Response> DeleteAsync(int id);
        Task<Response<CountryResponseDTO>> GetAsync(int id);
        Task<Response<List<CountryResponseDTO>>> GetAllAsync();
    }
}
