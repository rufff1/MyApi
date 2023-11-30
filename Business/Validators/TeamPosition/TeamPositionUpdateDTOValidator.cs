using Business.DTOs.TeamPosition.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Validators.TeamPosition
{
    public class TeamPositionUpdateDTOValidator : AbstractValidator<TeamPositionUpdateDTO>
    {
        public TeamPositionUpdateDTOValidator()
        {
            RuleFor(x => x.PositionName)
           .NotEmpty()
           .WithMessage("Ad daxil edilmelidir")
           .MaximumLength(50)
           .WithMessage("max chracter sayi 50 olmalidir");
        }
    }
}
