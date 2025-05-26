using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikeRental.DDD.Domain.Entities;
using FluentValidation;

namespace BikeRental.DDD.Domain.Validators
{
    public class RentalValidator : AbstractValidator<Rental>
    {
        public RentalValidator()
        {
            RuleFor(l => l.CustomerId)
               .NotNull()
               .NotEmpty().WithMessage("Campo obligatorio");

            RuleFor(l => l.BikeId)
               .NotNull()
               .NotEmpty().WithMessage("Campo obligatorio");
        }
    }
}
