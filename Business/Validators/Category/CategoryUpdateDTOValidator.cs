using Business.DTOs.Category.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Validators.Category
{
    public class CategoryUpdateDTOValidator : AbstractValidator<CategoryUpdateDTO>
    {
        public CategoryUpdateDTOValidator()
        {

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Ad daxil edilmelidir")
                .MaximumLength(20)
                .WithMessage("max chracter sayi 20 olmalidir");


            RuleFor(x => x.ImageFile)
              .NotEmpty()
              .WithMessage("image daxil edilmelidir");

        }

   

    }
  
}
