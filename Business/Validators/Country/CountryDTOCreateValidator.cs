using Business.DTOs.Country.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Validators.Country
{
    public class CountryDTOCreateValidator :AbstractValidator<CountryCreateDTO>
    {
        public CountryDTOCreateValidator()
        {
            RuleFor(x => x.CountryName)
               .NotEmpty()
               .WithMessage("Country adi bos ola bilmez")
               .MaximumLength(30)
               .WithMessage("max uzunlug 30 olmalidir");
        }
    }
}
