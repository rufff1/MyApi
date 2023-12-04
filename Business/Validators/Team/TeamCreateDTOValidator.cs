using Business.DTOs.Team.Request;
using DataAccess.Migrations;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Validators.Team
{
    public class TeamCreateDTOValidator :AbstractValidator<TeamCreateDTO>
    {
        public TeamCreateDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("name daxil edilmelidir")
                .MaximumLength(20)
                .WithMessage("name max 20 karakterden ibaret olmalidir");

            RuleFor(x => x.Age)
                .NotEmpty()
                .WithMessage("Age daxil edilmelidir");

            RuleFor(x => x.Info)
                .NotEmpty()
                .WithMessage("info daxil edilmelidir")
                .MaximumLength(1000)
                .WithMessage("name max 1000 karakterden ibaret olmalidir");


            RuleFor(x => x.ImageFile)
                .NotEmpty()
                .WithMessage("image daxil edilmelidir");

            RuleFor(x => x.PositionId)
                .NotEmpty()
                .WithMessage("position daxil edilmelidir");

            RuleFor(x => x.Email)
                 .NotEmpty()
                 .WithMessage("email daxil edilmelidir")
                 .EmailAddress()
                 .WithMessage("tip email olmalidir")
                 .MaximumLength(50)
                 .WithMessage("max 50 xarakter ola biler");



        }
    }
}
