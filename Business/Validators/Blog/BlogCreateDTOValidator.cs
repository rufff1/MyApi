using Business.DTOs.Blog.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Validators.Blog
{
    public class BlogCreateDTOValidator : AbstractValidator<BlogCreateDTO>
    {
        public BlogCreateDTOValidator()
        {
            RuleFor(x => x.Name)
               .NotEmpty()
               .WithMessage("Ad daxil edilmelidir")
               .MaximumLength(20)
               .WithMessage("max chracter sayi 20 olmalidir");

            RuleFor(x => x.ImageFile)
              .NotEmpty()
              .WithMessage("image daxil edilmelidir");

            RuleFor(x => x.Description)
              .NotEmpty()
              .WithMessage("Description daxil edilmelidir")
               .MaximumLength(1000)
               .WithMessage("max chracter sayi 20 olmalidir");

            RuleFor(x => x.CategoryId)
              .NotEmpty()
              .WithMessage("CategoryId daxil edilmelidir");

            RuleFor(x => x.BlogType)
             .IsInEnum()
             .WithMessage("Tip düzgün seçilməyib");
        }
    }
}
