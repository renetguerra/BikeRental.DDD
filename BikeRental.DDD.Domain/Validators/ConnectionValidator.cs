using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikeRental.DDD.Domain.Entities;
using FluentValidation;

namespace BikeRental.DDD.Domain.Validators
{
    public class ConnectionValidator : AbstractValidator<Connection>
    {
        public ConnectionValidator()
        {
            RuleFor(c => c.Username)
               .NotNull()
               .NotEmpty().WithMessage("Campo obligatorio");
        }
    }
}
