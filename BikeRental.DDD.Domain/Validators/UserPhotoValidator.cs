using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikeRental.DDD.Domain.Entities;
using FluentValidation;

namespace BikeRental.DDD.Domain.Validators
{
    public class UserPhotoValidator : AbstractValidator<UserPhoto>
    {
        public UserPhotoValidator()
        {
            RuleFor(p => p.UserId)
               .NotNull()
               .NotEmpty().WithMessage("Campo obligatorio");
        }
    }
}
