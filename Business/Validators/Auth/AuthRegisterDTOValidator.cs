using Business.DTOs.Auth.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Validators.Auth
{
    public class AuthRegisterDTOValidator : AbstractValidator<AuthRegisterDTO>
    {
        public AuthRegisterDTOValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Poçt ünvanı daxil edilməlidir");

            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("Poçt ünvanı düzgün formatda deyil");

            RuleFor(x => x.Password.Length)
                .GreaterThanOrEqualTo(8)
                .WithMessage("Şifrənin uzunluğu minimum 8 simvol olmalıdır");
        }
    }
}
