using AutoMapper;
using Azure.Core;
using Business.DTOs.Common;
using Business.DTOs.Tag.Response;
using Business.DTOs.TeamPosition.Request;
using Business.DTOs.TeamPosition.Response;
using Business.Exceptions;
using Business.Services.Abtraction;
using Business.Validators.TeamPosition;
using Common.Entities;
using DataAccess.Context;
using DataAccess.Repositories.Abstract;
using DataAccess.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Business.Services.Concered
{
    public class TeamPostionService : ITeamPositionService
    {
        public readonly IMapper _mapper;
        public readonly ITeamPositionRepository _teamPositionRepository;
        public readonly IUnitOfWork _unitOfWork;
        public readonly AppDbContext _context;
        private readonly ILogger<TeamPostionService> _logger;

        public TeamPostionService(IMapper mapper, ITeamPositionRepository teamPositionRepository, IUnitOfWork unitOfWork, AppDbContext context,ILogger<TeamPostionService> logger)
        {
            _mapper = mapper;
            _teamPositionRepository = teamPositionRepository;
            _unitOfWork = unitOfWork;
            _context = context;
            _logger = logger;

        }
        public async Task<Response> CreateAsync(TeamPositionCreateDTO model)
        {
            var result = await new TeamPositionCreateDTOValidator().ValidateAsync(model);

            if (!result.IsValid)
            {
                _logger.LogError("model validator error");
                throw new ValidationException(result.Errors);
            }

            var teamPosition = _mapper.Map<TeamPosition>(model);

            bool isExist = await _context.TeamPositions.AnyAsync(x => x.PositionName.ToLower() == model.PositionName.ToLower().Trim());

            if (isExist)
            {
                _logger.LogError("Bu position name artig var");

                throw new ValidationException("Bu position name artig var");
            }


            await _teamPositionRepository.CreateAsync(teamPosition);
            await _unitOfWork.CommitAsync();

            return new Response
            {
                Message = "Position ugurla yaradildi"
            };


        }

        public async Task<Response> DeleteAsync(int id)
        {

            var position  = await _teamPositionRepository.GetAsync(id);

            if (position == null) { _logger.LogError("position tapilmadi"); throw new ValidationException("position tapilmadi"); }

             _teamPositionRepository.Delete(position);
            await _unitOfWork.CommitAsync();


            return new Response { Message = "Position ugurla silindi" };
                
        }

        public async Task<Response<List<TeamPositionReponseDTO>>> GetAllAsync()
        {
            var response = await _teamPositionRepository.GetAllAsync();


            return new Response<List<TeamPositionReponseDTO>>
            {
                Data = _mapper.Map<List<TeamPositionReponseDTO>>(response),
                Message = "Butun Taglar getirildi"

            };
        }

        public async Task<Response<TeamPositionReponseDTO>> GetAsync(int id)
        {
            var response = await _teamPositionRepository.GetAsync(id);
            if (response == null) { _logger.LogError("position tapilmadi"); throw new ValidationException("position tapilmadi"); }

            return new Response<TeamPositionReponseDTO>
            {
                Data = _mapper.Map<TeamPositionReponseDTO>(response),
                Message = "position ugurlar getirildi"
            };
        }

        public async Task<Response> UpdateAsync(int id,TeamPositionUpdateDTO model)
        {
            var result = await new TeamPositionUpdateDTOValidator().ValidateAsync(model);

            if (!result.IsValid)
            {
                _logger.LogError("model validator error");

                throw new ValidationException(result.Errors);
            }

            var teamPosition = await _teamPositionRepository.GetAsync(id);

            if (teamPosition == null)
            {
                _logger.LogError("position tapilmadi");

                throw new NotFoundException("position tapilmadi");
            }

            _mapper.Map(model, teamPosition);

            bool isExist = await _context.TeamPositions.AnyAsync(c => c.PositionName.ToLower() == teamPosition.PositionName.ToLower().Trim());
            if (isExist)
            {
                _logger.LogError("Bu adla position var");
                throw new ValidationException("Bu adla position var");
            };

             teamPosition.ModifiedDate =  DateTime.Now;

               _teamPositionRepository.Update(teamPosition);
               
               await _unitOfWork.CommitAsync();

            return new Response
            {
                Message = "Position ugurla moified olundu"
            };


        }
    }
}
