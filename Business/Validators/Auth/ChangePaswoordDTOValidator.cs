using Business.DTOs.Auth.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Validators.Auth
{
    public class ChangePaswoordDTOValidator :AbstractValidator<ChangePaswoordDTO>
    {
        public ChangePaswoordDTOValidator() 
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("username daxil edim");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Hazirdaki parolu daxil edin");


            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .WithMessage("Yeni parolu daxil edin");

            RuleFor(x => x.ConfirmNewPassword)
                .NotEmpty()
                .WithMessage("yeni parolu tekrar daxil edin");
            
        }
    }
}
