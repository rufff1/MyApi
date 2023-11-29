
using Business.DTOs.Role.Request;
using Business.DTOs.Role.Response;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Validators.Role
{
    public class RoleDTOValidator : AbstractValidator<RoleCreateDTO>
    {
        public RoleDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Ad daxil edilməlidir");
        }
    }
}
