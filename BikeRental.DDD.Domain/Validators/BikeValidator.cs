using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikeRental.DDD.Domain.Entities;
using FluentValidation;

namespace BikeRental.DDD.Domain.Validators
{
    public class BikeValidator : AbstractValidator<Bike>
    {
        public BikeValidator()
        {
            RuleFor(b => b.Model)
                .NotNull()
                .NotEmpty().WithMessage("Mandatory value");                

            RuleFor(b => b.Brand)
                .NotNull()
                .NotEmpty().WithMessage("Mandatory value");

            RuleFor(b => b.Type)
               .NotNull()
               .NotEmpty().WithMessage("Mandatory value");
        }
    }
}
