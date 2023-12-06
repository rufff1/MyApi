

using AutoMapper;
using Business.DTOs.Blog.Request;
using Business.DTOs.Blog.Response;
using Business.DTOs.Common;
using Business.Exceptions;
using Business.Services.Abtraction;
using Business.Validators.Blog;
using Business.Validators.Product;
using Common.Entities;
using DataAccess.Context;
using DataAccess.Repositories.Abstract;
using DataAccess.Repositories.Concrete;
using DataAccess.UnitOfWork;
using DocumentFormat.OpenXml.Math;
using FirstMyApi.Extensions;
using FirstMyApi.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Business.Services.Concered
{
    public class BlogService : IBlogService
    {

        private readonly IBlogRepository _blogRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        private readonly ILogger<BlogService> _logger;


        private readonly IWebHostEnvironment _env;
        public BlogService(IBlogRepository blogRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IWebHostEnvironment env,
            AppDbContext context,
            ILogger<BlogService> logger

           )
        {
            _blogRepository = blogRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _env = env;
            _context = context;
            _logger = logger;

        }




        public async Task<Response> CreateAsync(BlogCreateDTO model)
        {

            var result = await new BlogCreateDTOValidator().ValidateAsync(model);

            if (!result.IsValid)
            {
                _logger.LogError("model validator error");
                throw new ValidationException(result.Errors);
            }
            var blog = _mapper.Map<Blog>(model);


            //bos list tuturug secilen taglari elave etmeye asagida elave edirik
            List<BlogTag> blogTags = new List<BlogTag>();

            foreach (int tagId in blog.TagIds)
            {
                if (blog.TagIds.Where(t => t == tagId).Count() > 1)
                {
                    _logger.LogError("Bir tagdan bir defe secilmelidir");

                    throw new ValidationException("Bir tagdan bir defe secilmelidir");

                }

                if (!await _context.Tags.AnyAsync(t => t.Id == tagId))
                {

                    _logger.LogError("secilen tag yalnisdir");

                    throw new ValidationException("secilen tag yalnisdir");
                }

                BlogTag blogTag = new BlogTag
                {
                    CreatedDate = DateTime.UtcNow,


                    TagId = tagId

                };

                //taglari bos liste add etdik
                blogTags.Add(blogTag);
            }

            if (!await _context.Categories.AnyAsync(c => c.Id == blog.CategoryId))
            {
                _logger.LogError("gelen category yalnisdir");

                throw new ValidationException("gelen category yalnisdir");
            }

            if (blog.ImageFile == null)
            {

                _logger.LogError("Image daxil edilmelidir");

                throw new ValidationException("Image daxil edilmelidir");
            }



            if (!blog.ImageFile.CheckFileSize(1000))
            {
                _logger.LogError("Image olcusu 1 mb cox olmamalidir");
                throw new ValidationException("Image olcusu 1 mb cox olmamalidir");
            }


            if (!blog.ImageFile.CheckFileType("image/jpeg"))
            {
                _logger.LogError("Image jpg tipi olmalidir");

                throw new ValidationException("Image jpg tipi olmalidir");

            }


            blog.Image = blog.ImageFile.CreateImage(_env, "img", "blog");
            blog.BlogTags = blogTags;

     

            await _blogRepository.CreateAsync(blog);
            await _unitOfWork.CommitAsync();






            _logger.LogInformation("blog ugurla yaradildi");
            return new Response
            {
                
                Message = "blog ugurla yaradildi"
            };
        }




        public async Task<Response> DeleteAsync(int id)
        {
            var blog = await _blogRepository.GetAsync(id);
            if (blog is null)
            {
                _logger.LogWarning("blog tapilmadi");

                throw new NotFoundException("blog tapilmadi");
            }

            _blogRepository.Delete(blog);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("blog uğurla silindi");
            return new Response
            {
                Message = "blog uğurla silindi"
            };
        }

        public async Task<Response<List<BlogGetCategoryTag>>> GetAllAsync()
        {


            var blogs = await _context.Blogs.Include(b=> b.BlogTags).ThenInclude(bt=> bt.Tag).Include(c=> c.Category).ToListAsync();

            if (blogs == null)
            {
                _logger.LogWarning("blog tapilmadi");

                throw new NotFoundException("blog tapilmadi");
            }

            return new Response<List<BlogGetCategoryTag>>
            {
                Data = _mapper.Map<List<BlogGetCategoryTag>>(blogs),
                Message = "ugurlu alindi"
            };

        }

        public async Task<Response<BlogResponseDTO>> GetAsync(int id)
        {
            var blog = await _blogRepository.GetAsync(id);

            if (blog == null)
            {
                _logger.LogWarning("blog tapilmadi");
                throw new NotFoundException("blog tapilmadi");
            }


            _logger.LogInformation("blog tapildi");
            return new Response<BlogResponseDTO>
            {
                Data = _mapper.Map<BlogResponseDTO>(blog),
                Message = "ugurlu alindi"
            };

        }

        public async Task<Response> UpdateAsync(int id, BlogUpdateDTO model)
        {
            var result = await new BlogUpdateDTOValidator().ValidateAsync(model);
            if (!result.IsValid)
            {
                _logger.LogWarning("model validator error");

                throw new ValidationException(result.Errors);

            }

            var existBlog =  await _blogRepository.GetAsync(id);    
            if (existBlog is null)
            {
                _logger.LogError("blog tapilmadi");

                throw new NotFoundException("blog tapılmadı");
            }



            _mapper.Map(model, existBlog);

            if (!await _context.Categories.AnyAsync(t => t.Id == existBlog.CategoryId))
            {
                _logger.LogWarning("duzgun category secin");

                throw new ValidationException("duzgun category secin");
            }

            _context.BlogTags.RemoveRange(existBlog.BlogTags);




            List<BlogTag> blogTags = new List<BlogTag>();

            foreach (int tagId in existBlog.TagIds)
            {
                if (existBlog.TagIds.Where(t => t == tagId).Count() > 1)
                {
                    _logger.LogWarning("Bir tagdan bir defe secilmelidir");
                    throw new ValidationException("Bir tagdan bir defe secilmelidir");

                }

                if (!await _context.Tags.AnyAsync(t => t.Id == tagId))
                {
                    _logger.LogWarning("secilen tag yalnisdir");
                    throw new ValidationException("secilen tag yalnisdir");

                }

                BlogTag blogTag = new BlogTag
                {
                    CreatedDate = DateTime.UtcNow,


                    TagId = tagId

                };

                //taglari bos liste add etdik
                blogTags.Add(blogTag);
            }


            if (existBlog.ImageFile == null)
            {
                _logger.LogWarning("Image daxil edilmelidir");


                throw new ValidationException("Image daxil edilmelidir");
            }



            if (!existBlog.ImageFile.CheckFileSize(1000))
            {
                _logger.LogWarning("Image olcusu 1 mb cox olmamalidir");
                throw new ValidationException("Image olcusu 1 mb cox olmamalidir");
            }


            if (!existBlog.ImageFile.CheckFileType("image/jpeg"))
            {
                _logger.LogWarning("Image jpg tipi olmalidir");

                throw new ValidationException("Image jpg tipi olmalidir");

            }



            Helper.DeleteFile(_env, existBlog.Image, "img", "blog");
            existBlog.Image = existBlog.ImageFile.CreateImage(_env, "img", "blog");
            existBlog.BlogTags = blogTags;
            existBlog.ModifiedDate = DateTime.Now;
            existBlog.CategoryId = existBlog.CategoryId;
            existBlog.Name = existBlog.Name;
            existBlog.Description = existBlog.Description;
            try
            {

                _blogRepository.Update(existBlog);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                int a = 32;
            }


            _logger.LogInformation("blog uğurla redaktə olundu");
            return new Response
            {
                Message = "blog uğurla redaktə olundu"
            };
        }
    }
}
