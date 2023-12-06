
using AutoMapper;
using Business.DTOs.Common;
using Business.DTOs.Product.Request;
using Business.DTOs.Product.Response;
using Business.Exceptions;
using Business.Services.Abtraction;
using Business.Validators.Product;
using Common.Entities;
using DataAccess.Context;
using DataAccess.Repositories.Abstract;
using DataAccess.UnitOfWork;
using FirstMyApi.Extensions;
using FirstMyApi.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Reflection.Metadata;
using System.Web.Http.ModelBinding;

namespace Business.Services.Concered
{
    public class ProductService : IProductService 
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
       private readonly ILogger<ProductService> _logger;

        private readonly IWebHostEnvironment _env;
        public ProductService(IProductRepository productRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IWebHostEnvironment env,
            AppDbContext context,
            ILogger<ProductService> logger
        
           )
        {
            _productRepository = productRepository ;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _env = env;
            _logger = logger;
            _context = context;
           
        }

        public async Task<Response> CreateAsync(ProductCreateDTO model)
        {
            var result = await new ProductCreateDTOValidator().ValidateAsync(model);
            if (!result.IsValid)
            {
                _logger.LogWarning("model validator error");
               
                throw new ValidationException(result.Errors);
            }
            var product = _mapper.Map<Product>(model);


            //bos list tuturug secilen taglari elave etmeye asagida elave edirik
            List<ProductTag> productTags = new List<ProductTag>();

            foreach (int tagId in product.TagIds)
            {
                
                if (product.TagIds.Where(t => t == tagId).Count() > 1)
                {
                      _logger.LogWarning("Bir tagdan bir defe secilmelidir");

                    throw new ValidationException("Bir tagdan bir defe secilmelidir");

                }

                if (!await _context.Tags.AnyAsync(t => t.Id == tagId))
                {

                    _logger.LogError("secilen tag yalnisdir");

                    throw new ValidationException("secilen tag yalnisdir");
                }

                ProductTag productTag = new ProductTag
                {
                    CreatedDate = DateTime.UtcNow,
                   
                   
                    TagId = tagId

                };

                //taglari bos liste add etdik
                productTags.Add(productTag);
            }

       

            if (!await _context.Categories.AnyAsync(c =>  c.Id == product.CategoryId))
            {
                _logger.LogError("gelen category yalnisdir");
                throw new ValidationException("gelen category yalnisdir");
            }

            if (product.ImageFile == null)
            {
                _logger.LogError("Image daxil edilemlidir");
                

                throw new ValidationException("Image daxil edilmelidir");
            }



            if (!product.ImageFile.CheckFileSize(1000))
            {
                _logger.LogError("Image olcusu 1 mb cox olmamalidir");
                throw new ValidationException("Image olcusu 1 mb cox olmamalidir");
            }
            

            if (!product.ImageFile.CheckFileType("image/jpeg"))
            {
                _logger.LogError("Image jpg tipi olmalidir");
               
                throw new ValidationException("Image jpg tipi olmalidir");

            }


            product.Image = product.ImageFile.CreateImage(_env, "img", "product");
            product.ProductTags = productTags;


            await _productRepository.CreateAsync(product);
            await _unitOfWork.CommitAsync();






            return new Response
            {
                
                Message = "product ugurla yaradildi"
            };
        }

    

        public async Task<Response> DeleteAsync(int id)
        {
            var product = await _productRepository.GetAsync(id);
            if (product is null)
            {
                _logger.LogError("Mehsul tapilmadi");

                throw new NotFoundException("Mehsul tapilmadi");
            }

            _productRepository.Delete(product);
            await _unitOfWork.CommitAsync();

            return new Response
            {
                Message = "product uğurla silindi"
            };

        }

     
        public async Task<Response<ProductResponseDTO>> GetAsync(int id)
        {
            var productWithCategory = await _context.Products.Include(x=> x.Category).FirstOrDefaultAsync(x => x.Id == id);

            if (productWithCategory is null)
            {
                _logger.LogError("product tapilmadi");
                throw new NotFoundException("product tapilmadi");
            }

            return new Response<ProductResponseDTO>
            {
                Data = _mapper.Map<ProductResponseDTO>(productWithCategory),
                Message = $"id-si {id} olan product tapildi"
            };
        }


        public async Task<Response<List<ProductGetTags>>> GetAllAsync()
        {

            var products= await _context.Products
                .Include(x=> x.Category)
               .Include(p=> p.ProductTags).ThenInclude(pt=> pt.Tag)
                .ToListAsync();
           

            if (products is null)
            {
                _logger.LogError("product yoxdur");


                throw new NotFoundException("product yoxdur");
            }


           
          
                return new Response<List<ProductGetTags>>
                {

                    Data = _mapper.Map<List<ProductGetTags>>(products),

                    Message = "All products retrieved successfully."
                };
            
         
         


        }

        public async Task<Response> UpdateAsync(int id, ProductUpdateDTO model)
        {
            var result =await new ProductUpdateDTOValidator().ValidateAsync(model);
            if (!result.IsValid)
            {
                _logger.LogError("model validator error");

                throw new ValidationException(result.Errors);
            }

            var existProduct = await _context.Products
              .Include(p => p.ProductTags).ThenInclude(pt => pt.Tag)
              .FirstOrDefaultAsync(x=> x.Id == id);
            if (existProduct is null)
            {
                _logger.LogError("Product tapılmadı");

                throw new NotFoundException("Product tapılmadı");
            }

         

            _mapper.Map(model, existProduct);

            if (!await _context.Categories.AnyAsync(t =>  t.Id == existProduct.CategoryId))
            {
                throw new ValidationException("duzgun category secin");
            }

            _context.ProductTags.RemoveRange(existProduct.ProductTags);
          



            List<ProductTag> productTags = new List<ProductTag>();

            foreach (int tagId in existProduct.TagIds)
            {
                if (existProduct.TagIds.Where(t => t == tagId).Count() > 1)
                {
                    _logger.LogError("Bir tagdan bir defe secilmelidir");
                    throw new ValidationException("Bir tagdan bir defe secilmelidir");

                }

                if (!await _context.Tags.AnyAsync(t => t.Id == tagId))
                {
                    _logger.LogError("secilen tag yalnisdir");

                    throw new ValidationException("secilen tag yalnisdir");

                }

                ProductTag productTag = new ProductTag
                {
                    CreatedDate = DateTime.UtcNow,


                    TagId = tagId

                };

                //taglari bos liste add etdik
                productTags.Add(productTag);
            }


            if (existProduct.ImageFile == null)
            {
                _logger.LogError("Image daxil edilmelidir");


                throw new ValidationException("Image daxil edilmelidir");
            }



            if (!existProduct.ImageFile.CheckFileSize(1000))
            {
                _logger.LogError("Image olcusu 1 mb cox olmamalidir");
                throw new ValidationException("Image olcusu 1 mb cox olmamalidir");
            }


            if (!existProduct.ImageFile.CheckFileType("image/jpeg"))
            {
                _logger.LogError("Image jpg tipi olmalidir");
                throw new ValidationException("Image jpg tipi olmalidir");

            }



            Helper.DeleteFile(_env, existProduct.Image, "img", "product");
            existProduct.Image = existProduct.ImageFile.CreateImage(_env, "img", "product");
            existProduct.ProductTags =productTags;
            existProduct.ModifiedDate = DateTime.Now;
            existProduct.CategoryId = existProduct.CategoryId;
            existProduct.Name = existProduct.Name;
            existProduct.Description = existProduct.Description;
           
                _productRepository.Update(existProduct);
                await _unitOfWork.CommitAsync();
            
          

            return new Response
            {
                Message = "product uğurla redaktə olundu"
            };
        }

  
    }
}
