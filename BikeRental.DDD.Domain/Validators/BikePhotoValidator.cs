using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikeRental.DDD.Domain.Entities;
using FluentValidation;

namespace BikeRental.DDD.Domain.Validators
{
    public class BikePhotoValidator : AbstractValidator<BikePhoto>
    {
        public BikePhotoValidator()
        {
            RuleFor(p => p.BikeId)
            .NotNull().WithMessage("BikeId is mandatory.");

            RuleFor(p => p.Url)
                .NotEmpty().WithMessage("The photo URL is required.");
        }
    }
}
