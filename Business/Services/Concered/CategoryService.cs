

using AutoMapper;
using Business.DTOs.Category.Request;
using Business.DTOs.Category.Response;
using Business.DTOs.Common;
using Business.DTOs.Product.Request;
using Business.Exceptions;
using Business.Services.Abtraction;
using Business.Validators.Category;
using Common.Entities;
using DataAccess.Context;
using DataAccess.Repositories.Abstract;
using DataAccess.UnitOfWork;
using FirstMyApi.Extensions;
using FirstMyApi.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Numerics;
using System.Web.Http.ModelBinding;

namespace Business.Services.Concered
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly AppDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;
        

        private readonly IWebHostEnvironment _env;
        public CategoryService(ICategoryRepository categoryRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IWebHostEnvironment env,
            AppDbContext context,
            ILogger<CategoryService> logger

           )
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _env = env;
            _context = context;
            _logger = logger;
         
        }

        public async Task<Response> CreateAsync(CategoryCreateDTO model)
        {
            var result = await new CategoryCreateDTOValidator().ValidateAsync(model);
            if (!result.IsValid)
            {
                _logger.LogError("model validator error");
                throw new ValidationException(result.Errors);
            }

            var category = _mapper.Map<Category>(model);


          


        

            bool categoryName = await _context.Categories.AnyAsync(x => x.Name.ToLower().Trim() == category.Name.ToLower().Trim());

            if (categoryName)
            {
                _logger.LogWarning("Bu adla category var");
                throw new ValidationException("Bu adla category var");
            }

            if (category.ImageFile == null)
            {
                
                    _logger.LogWarning("Image daxil edilmelidir");

                    throw new ValidationException("Image daxil edilmelidir");
            }



            if (!category.ImageFile.CheckFileSize(1000))
            {
                _logger.LogWarning("Image olcusu max 1 mb olamlidir");
                throw new ValidationException("Image olcusu max 1 mb olamlidir");

            }
            if (!category.ImageFile.CheckFileType("image/jpeg"))
            {
                _logger.LogWarning("Image jpg tipi olmalidir");
               
                throw new ValidationException("Image jpg tipi olmalidir");

            }


            category.Image = category.ImageFile.CreateImage(_env, "img", "category");



                await _categoryRepository.CreateAsync(category);
                await _unitOfWork.CommitAsync();






            return new Response
            {
                Message = "category ugurla yaradildi"
            };
        }

     

        public async Task<Response> DeleteAsync(int id)
        {
            var category = await _categoryRepository.GetAsync(id);

            if(category is null)
            {
                _logger.LogError("category tapilmadi\"");
                throw new NotFoundException("category tapilmadi");
            }

               _categoryRepository.Delete(category);
               await _unitOfWork.CommitAsync();

            return new Response
            {
                Message = "Category uğurla silindi"
            };

        }

        public async Task<Response<List<CategoryDTO>>> GetAllAsync()
        {

            var categoryWithPro = await _context.Categories.Include(x => x.Products).ToListAsync();
            if (categoryWithPro is null)
            {
                _logger.LogWarning("hec bir category tapılmadı");
                throw new NotFoundException("hec bir category tapılmadı");
            }

           

            return new Response<List<CategoryDTO>>
            {
                Data = _mapper.Map<List<CategoryDTO>>(categoryWithPro),

                Message = "All categories retrieved successfully."
            };


        }

        public async Task<Response<CategoryDTO>> GetAsync(int id)
        {
            var categoryWithPro = await _context.Categories.Include(x => x.Products).FirstOrDefaultAsync(x=> x.Id== id);
            if (categoryWithPro is null)
            {
                _logger.LogWarning(" category tapılmadı");

                throw new NotFoundException("Catgory tapılmadı");
            }

            return new Response<CategoryDTO>
            {
                Data = _mapper.Map<CategoryDTO>(categoryWithPro),

                Message = "categorie retrieved successfully."
            };
        }


        public async Task<Response> UpdateAsync(CategoryUpdateDTO model)
        {
            var result = await new CategoryUpdateDTOValidator().ValidateAsync(model);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);

            var existCategory = await _categoryRepository.GetAsync(model.Id);
            if (existCategory is null)
            {
                _logger.LogWarning("category tapılmadı");

                throw new NotFoundException("category tapılmadı");
            }

            _mapper.Map(model, existCategory);



            bool isExist =await _context.Categories.AnyAsync(c => c.Name.ToLower() == existCategory.Name.ToLower().Trim());
            if (isExist)
            {
                _logger.LogWarning("bu adla category var");

                throw new ValidationException("Bu adla category var");  
            };




            if (existCategory.ImageFile == null)
            {
                _logger.LogError("Image daxil edilmelidir");

                throw new ValidationException("Image daxil edilmelidir");
            }



            if (!existCategory.ImageFile.CheckFileSize(1000))
            {
                _logger.LogError("Image olcusu max 1 mb olamlidir");

                throw new ValidationException("Image olcusu max 1 mb olamlidir");

            }
            if (!existCategory.ImageFile.CheckFileType("image/jpeg"))
            {
                _logger.LogError("Image jpg tipi olmalidir");

                throw new ValidationException("Image jpg tipi olmalidir");

            }
            Helper.DeleteFile(_env, existCategory.Image, "img", "category");
            existCategory.Image = model.ImageFile.CreateImage(_env,  "img", "category");
            existCategory.ModifiedDate = DateTime.Now;

            _categoryRepository.Update(existCategory);
            await _unitOfWork.CommitAsync();

            return new Response
            {
                Message = "category uğurla redaktə olundu"
            };
        }

       
    }
}
