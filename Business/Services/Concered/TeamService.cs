
using AutoMapper;
using Business.DTOs.Team.Request;
using Business.DTOs.Team.Response;
using Business.DTOs.Common;

using Business.Exceptions;
using Business.Services.Abtraction;
using Business.Validators.Team;
using Common.Entities;
using DataAccess.Context;
using DataAccess.Repositories.Abstract;
using DataAccess.UnitOfWork;
using FirstMyApi.Extensions;
using FirstMyApi.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Common.Constants;
using Microsoft.Extensions.Logging;

namespace Business.Services.Concered
{
    public class TeamService : ITeamService
    {
        public readonly IMapper _mapper;
        private readonly ITeamRepository _teamRepository;
        private readonly IUnitOfWork _unitOfWork;
       private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<TeamService> _logger;
        public TeamService(IMapper mapper, ITeamRepository teamRepository ,IUnitOfWork unitOfWork,IWebHostEnvironment env,AppDbContext context, ILogger<TeamService> logger )
        {
           
            _mapper = mapper;
            _teamRepository = teamRepository;
            _unitOfWork = unitOfWork;
            _env = env;
            _context = context;
            _logger = logger;
        }


        public async Task<Response> CreateAsync(TeamCreateDTO model)
        {
            var result = await new TeamCreateDTOValidator().ValidateAsync(model);

            if (!result.IsValid) {_logger.LogError("model validator error"); throw new ValidationException(result.Errors); }

            var team = _mapper.Map<Team>(model);


            bool teamEmail = await _context.Teams.AnyAsync(x => x.Email.ToLower().Trim() == model.Email.ToLower().Trim());

            if (teamEmail)
            {
                _logger.LogError("Bu email artig var");
                throw new ValidationException("Bu email artig var");
            }


            if (team.ImageFile == null)
            {
                _logger.LogError("Image daxil edilmelidir");
                throw new ValidationException("Image daxil edilmelidir");
            }



            if (!team.ImageFile.CheckFileSize(1000))
            {
                _logger.LogError("Image olcusu max 1 mb olamlidir");
                throw new ValidationException("Image olcusu max 1 mb olamlidir");

            }
            if (!team.ImageFile.CheckFileType("image/jpeg"))
            {
                _logger.LogError("Image jpg tipi olmalidir");

                throw new ValidationException("Image jpg tipi olmalidir");

            }


            team.Image = team.ImageFile.CreateImage(_env, "img", "team");
            team.Email = team.Email.Trim();
            await _teamRepository.CreateAsync(team);
            await _unitOfWork.CommitAsync();


         

            return new Response
            {
                Message = "team ugurla yaradildi"
            };
            
        }

        public async Task<Response> DeleteAsync(int id)
        {
            var team = await _teamRepository.GetAsync(id);

            if (team == null)
            {
                _logger.LogError("team tapilmadi");
                throw new ValidationException("team tapilmadi");
            }

             _teamRepository.Delete(team);
            await _unitOfWork.CommitAsync();

            return new Response
            {
                Message = "team ugurla silindi"
            };
        }

        public async Task<Response<List<TeamResponseDTO>>> GetAllAsync()
        {
            var response = await _context.Teams.Include(x=> x.Country).ToListAsync();

            if (response.Count() < 1)
            {
            
                throw new NotFoundException("team movcud deyil");
            }


            return new Response<List<TeamResponseDTO>>
            {
                Data = _mapper.Map<List<TeamResponseDTO>>(response),
                Message = "teamlar ugurla getirildi"
            };
        }

        public async Task<Response<TeamResponseDTO>> GetAsync(int id)
        {
            var response = await _teamRepository.GetAsync(id);
            if (response == null) throw new NotFoundException("team tapilmadi");

           

            return new Response<TeamResponseDTO>
            {
                Data = _mapper.Map<TeamResponseDTO>(response),
                Message = "team ugurla getirildi"
            };

        }

        public async Task<Response> UpdateAsync(TeamUpdateDTO model)
        {
            var result = await new TeamUpdateDTOValidator().ValidateAsync(model);
           
                if (!result.IsValid) {_logger.LogError("model validator error"); throw new ValidationException(result.Errors); }

            var existedteam = await _teamRepository.GetAsync(model.Id);

            if (existedteam == null)
            {
                _logger.LogError("team tapilmadi");
                throw new NotFoundException("team tapilmadi");
            }
            _mapper.Map(model, existedteam);


            if (existedteam.ImageFile == null)
                {
                _logger.LogError("Image daxil edilmelidir");
                    throw new ValidationException("Image daxil edilmelidir");
                }
            
          




            if (!existedteam.ImageFile.CheckFileSize(1000))
            {
                _logger.LogError("Image olcusu max 1 mb olamlidir");
                throw new ValidationException("Image olcusu max 1 mb olamlidir");

            }
            if (!existedteam.ImageFile.CheckFileType("image/jpeg"))
            {
                _logger.LogError("Image jpg tipi olmalidir");

                throw new ValidationException("Image jpg tipi olmalidir");

            }

            Helper.DeleteFile(_env, existedteam.Image, "img", "category");
            existedteam.Image = model.ImageFile.CreateImage(_env, "img", "category");



            existedteam.ModifiedDate = DateTime.Now;   

            
            _teamRepository.Update(existedteam);

            await _unitOfWork.CommitAsync();

            return new Response
            {
                Message = "team ugurla modified olundu"
            };

        }
    }
}
