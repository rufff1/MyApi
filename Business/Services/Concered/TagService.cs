using AutoMapper;
using Business.DTOs.Common;
using Business.DTOs.Tag.Request;
using Business.DTOs.Tag.Response;
using Business.Exceptions;
using Business.Services.Abtraction;
using Business.Validators.Tag;
using Common.Entities;
using DataAccess.Context;
using DataAccess.Repositories.Abstract;
using DataAccess.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Business.Services.Concered
{
    public class TagService : ITagService
    {
        public readonly IMapper _mapper;
        public readonly ITagRepository _tagRepository;
        public readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;


        public TagService(IMapper mapper, ITagRepository tagRepository, IUnitOfWork unitOfWork,AppDbContext context)
        {
            _mapper = mapper;
            _tagRepository = tagRepository;
                _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<Response> CreateAsync(TagCreateDTO model)
        {
            var result = await new TagCreateDTOValidator().ValidateAsync(model);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            var tag = _mapper.Map<Tag>(model);


            bool tagName = await _context.Tags.AnyAsync(x => x.Name.ToLower() == model.Name.ToLower());

            if (tagName)
            {
                throw new ValidationException("bu tag name movcuddur");
            }

           await _tagRepository.CreateAsync(tag);
           await _unitOfWork.CommitAsync();


            return new Response
            {
                Message = "tag ugurla yaradildi"
            };
            


        }

        public async Task<Response> DeleteAsync(int id)
        {
            var tag = await _tagRepository.GetAsync(id);

            if (tag == null)
            {
                throw new NotFoundException("tag tapilmadi");

            }

             _tagRepository.Delete(tag);

            await _unitOfWork.CommitAsync();


            return new Response
            {
                Message= "tagb ugurla silindi"
            };
        }

        public async Task<Response<List<TagResponseDTO>>> GetAllAsync()
        {
            var response = await _tagRepository.GetAllAsync();
          

            return new Response<List<TagResponseDTO>>
            {
                Data = _mapper.Map<List<TagResponseDTO>>(response),
                Message = "Butun Taglar getirildi"
              

            };
        }

        public async Task<Response<TagResponseDTO>> GetAsync(int id)
        {
            var response = await _tagRepository.GetAsync(id);
            if (response == null)
                throw new NotFoundException("Tag Tapilmadi");

            return new Response<TagResponseDTO>
            {
                Data = _mapper.Map<TagResponseDTO>(response),
                Message = "Tag tapildi"
            };
          
        }

        public async Task<Response> UpdateAsync(int id, TagUpdateDTO model)
        {
            var result = await new TagUpdateDTOValidator().ValidateAsync(model);

            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }


            var tag = await _tagRepository.GetAsync(id);


            if (tag == null)
                throw new NotFoundException("tag tapilmadi");


              _mapper.Map(model, tag);




            bool isExist =await _context.Tags.AnyAsync(c => c.Name.ToLower() == tag.Name.ToLower().Trim());
            if (isExist)
            {
                throw new ValidationException("Bu adla tag var");
            };


            tag.ModifiedDate = DateTime.Now;
        
                _tagRepository.Update(tag);
            try
            {
                await _unitOfWork.CommitAsync();

            }
            catch (Exception ex)
            {

                throw new ValidationException("nese duzgun getmedi");
            }

            return new Response
            {
                Message = "tag ugurla modified olundu"
            };



        }

      
    }
}
