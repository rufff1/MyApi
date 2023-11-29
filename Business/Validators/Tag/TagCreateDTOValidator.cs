using Business.DTOs.Tag.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Validators.Tag
{
    public class TagCreateDTOValidator :AbstractValidator<TagCreateDTO>
    {
        public TagCreateDTOValidator()
        {
            RuleFor(x => x.Name)
               .NotEmpty()
               .WithMessage("Ad daxil edilmelidir")
               .MaximumLength(20)
               .WithMessage("max chracter sayi 20 olmalidir");

        }
    }
}
