using Business.DTOs.Country.Request;
using FluentValidation;


namespace Business.Validators.Country
{
    public class CountryDTOUpdateValidator :AbstractValidator<CountryUpdateDTO>
    {

        public CountryDTOUpdateValidator()
        {
            RuleFor(x => x.CountryName)
               .NotEmpty()
               .WithMessage("Country adi bos ola bilmez")
               .MaximumLength(30)
               .WithMessage("max uzunlug 30 olmalidir");
        }
    }
}
