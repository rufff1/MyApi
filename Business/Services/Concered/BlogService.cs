

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
using FirstMyApi.Extensions;
using FirstMyApi.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace Business.Services.Concered
{
    public class BlogService : IBlogService
    {

        private readonly IBlogRepository _blogRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;


        private readonly IWebHostEnvironment _env;
        public BlogService(IBlogRepository blogRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IWebHostEnvironment env,
            AppDbContext context

           )
        {
            _blogRepository = blogRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _env = env;
            _context = context;

        }




        public async Task<Response> CreateAsync(BlogCreateDTO model)
        {

            var result = await new BlogCreateDTOValidator().ValidateAsync(model);

            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
            var blog = _mapper.Map<Blog>(model);


            //bos list tuturug secilen taglari elave etmeye asagida elave edirik
            List<BlogTag> blogTags = new List<BlogTag>();

            foreach (int tagId in blog.TagIds)
            {
                if (blog.TagIds.Where(t => t == tagId).Count() > 1)
                {
                    throw new ValidationException("Bir tagdan bir defe secilmelidir");

                }

                if (!await _context.Tags.AnyAsync(t => t.Id == tagId))
                {


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
                throw new ValidationException("gelen category yalnisdir");
            }

            if (blog.ImageFile == null)
            {


                throw new ValidationException("Image daxil edilmelidir");
            }



            if (!blog.ImageFile.CheckFileSize(1000))
            {
                throw new ValidationException("Image olcusu 1 mb cox olmamalidir");
            }


            if (!blog.ImageFile.CheckFileType("image/jpeg"))
            {

                throw new ValidationException("Image jpg tipi olmalidir");

            }


            blog.Image = blog.ImageFile.CreateImage(_env, "img", "blog");
            blog.BlogTags = blogTags;

     

            await _blogRepository.CreateAsync(blog);
            await _unitOfWork.CommitAsync();






            return new Response
            {

                Message = "blog ugurla yaradildi"
            };
        }




        public async Task<Response> DeleteAsync(int id)
        {
            var blog = await _blogRepository.GetAsync(id);
            if (blog is null)
                throw new NotFoundException("Mehsul tapilmadi");

            _blogRepository.Delete(blog);
            await _unitOfWork.CommitAsync();

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
                throw new NotFoundException("blog tapilmadi");
            }

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
                throw new ValidationException(result.Errors);

            var existBlog =  await _blogRepository.GetAsync(id);    
            if (existBlog is null)
                throw new NotFoundException("blog tapılmadı");



            _mapper.Map(model, existBlog);

            if (!await _context.Categories.AnyAsync(t => t.Id == existBlog.CategoryId))
            {
                throw new ValidationException("duzgun category secin");
            }

            _context.BlogTags.RemoveRange(existBlog.BlogTags);




            List<BlogTag> blogTags = new List<BlogTag>();

            foreach (int tagId in existBlog.TagIds)
            {
                if (existBlog.TagIds.Where(t => t == tagId).Count() > 1)
                {
                    throw new ValidationException("Bir tagdan bir defe secilmelidir");

                }

                if (!await _context.Tags.AnyAsync(t => t.Id == tagId))
                {
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


                throw new ValidationException("Image daxil edilmelidir");
            }



            if (!existBlog.ImageFile.CheckFileSize(1000))
            {
                throw new ValidationException("Image olcusu 1 mb cox olmamalidir");
            }


            if (!existBlog.ImageFile.CheckFileType("image/jpeg"))
            {

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

            return new Response
            {
                Message = "blog uğurla redaktə olundu"
            };
        }
    }
}
