using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikeRental.DDD.Domain.Entities;
using FluentValidation;

namespace BikeRental.DDD.Domain.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(p => p.UserName)
                .NotNull()
                .NotEmpty().WithMessage("Mandatory value")
                .Length(5, 20).WithMessage($"Minimum {0} and maximum {1} characters");

            //RuleFor(p => p.Introduction)
            //    .NotNull()
            //    .NotEmpty().WithMessage("Mandatory value")
            //    .MaximumLength(500).WithMessage($"Max {0} characters");

            //RuleFor(p => p.Address)
            //   .NotNull()
            //   .NotEmpty().WithMessage("Mandatory value");
        }
    }
}
