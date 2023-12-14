using AutoMapper;
using Business.DTOs.Common;
using Business.DTOs.Country.Request;
using Business.DTOs.Country.Response;
using Business.Exceptions;
using Business.Services.Abtraction;
using Business.Validators.Country;
using Common.Constants.Country;
using DataAccess.Context;
using DataAccess.Repositories.Abstract;
using DataAccess.Repositories.Concrete;
using DataAccess.UnitOfWork;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Concered
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;
        private readonly ILogger<CountryService> _logger;   
        public CountryService(ICountryRepository countryRepository,IMapper mapper,IUnitOfWork unitOfWork,AppDbContext context,ILogger<CountryService > logger)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;  
            _context = context;
            _logger = logger;
        }

        public async Task<Response> CreateAsync(CountryCreateDTO model)
        {
            var result = await new CountryDTOCreateValidator().ValidateAsync(model);

            if (!result.IsValid)
            {
                _logger.LogError("model validator error");
                throw new ValidationException(result.Errors);
            }

            var country = _mapper.Map<Country>(model);



            bool countryName = await _context.Countries.AnyAsync(x => x.CountryName.ToLower().Trim()  == model.CountryName.ToLower().Trim());

            if (countryName)
            {
                _logger.LogError("bu country adi artig movcuddur");
                throw new ValidationException("bu country adi artig movcuddur");
            }


            await _countryRepository.CreateAsync(country);
            await _unitOfWork.CommitAsync();



            _logger.LogInformation("Ugurla olke yaradildi");
            return new Response
            {
                Message = "Ugurla olke yaradildi"
            };

        }

        public async Task<Response> DeleteAsync(int id)
        {
            var country = await _countryRepository.GetAsync(id);

            if (country == null)
            {
                _logger.LogError("country tapilmadi");

                throw new NotFoundException("country tapilmadi");
            }

             _countryRepository.Delete(country);
            await _unitOfWork.CommitAsync();


            _logger.LogInformation($"{nameof(Country)} deleted");
            return new Response
            {
                Message = "country ugurla silindi"
            };


        }

        public async Task<Response<List<CountryResponseDTO>>> GetAllAsync()
        {
            var response = await _countryRepository.GetAllAsync();

            if (response == null)
            {
                _logger.LogError("country tapilmadi");
                throw new NotFoundException("country tapilmadi");
            }


            _logger.LogInformation("olkeler ugurla getirildi");
            return new Response<List<CountryResponseDTO>>
            {
                Data = _mapper.Map<List<CountryResponseDTO>>(response),
                Message = "olkeler ugurla getirildi"
            };

        }

        public async Task<Response<CountryResponseDTO>> GetAsync(int id)
        {
            var response = await _countryRepository.GetAsync(id);
            if (response == null)
            {
                _logger.LogError("country tapilmadi");
                throw new NotFoundException("country tapilmadi");
            }


            _logger.LogInformation($"Could not find country: {id}");
            return new Response<CountryResponseDTO>
            {
                Data = _mapper.Map<CountryResponseDTO>(response),
                Message = "country tapildi"
            };

        }

        public async Task<Response> UpdateAsync(int id, CountryUpdateDTO model)
        {
            var result = await new CountryDTOUpdateValidator().ValidateAsync(model);
            if (!result.IsValid)
            {
                _logger.LogWarning("model validator error");
               
                throw new ValidationException(result.Errors);
            }

            var existCountry = await _countryRepository.GetAsync(id);

            if (existCountry == null)
            {
                _logger.LogError("country tapilmadi");
                throw new NotFoundException("country tapilmadi");
            }



            bool isExist = await _context.Countries.AnyAsync(c => c.CountryName.ToLower() == model.CountryName.ToLower().Trim());
            if (isExist)
            {
                _logger.LogError("Bu adla country var");
                throw new ValidationException("Bu adla country var");
            };

            existCountry.ModifiedDate = DateTime.Now;

                 _countryRepository.Update(existCountry);
            
                    await _unitOfWork.CommitAsync();



            _logger.LogInformation($"Country updated: {model.CountryName}");
            return new Response
            {
                Message = "country ugurla modified olundu"
            };



        }
    }
}
