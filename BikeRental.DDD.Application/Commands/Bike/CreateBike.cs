using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikeRental.DDD.Domain;
using BikeRental.DDD.Domain.Entities;
using BikeRental.DDD.Domain.Events;
using Carter;
using Carter.ModelBinding;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace BikeRental.DDD.Application.Commands.Bike
{
    public class CreateBike
    {        
        public record CreateBikeCommand(BikeRental.DDD.Domain.Entities.Bike Bike) : IRequest<IResult>;

        // Handler
        public class CreateBikeHandler : IRequestHandler<CreateBikeCommand, IResult>
        {            
            private readonly IValidator<BikeRental.DDD.Domain.Entities.Bike> _validator;
            private readonly IUnitOfWork _unitOfWork;
            
            public CreateBikeHandler(IUnitOfWork unitOfWork, IValidator<BikeRental.DDD.Domain.Entities.Bike> validator)
            {
                _unitOfWork = unitOfWork;
                _validator = validator;
            }
            public async Task<IResult> Handle(CreateBikeCommand request, CancellationToken cancellationToken)
            {
                var result = _validator.Validate(request.Bike);
                if (!result.IsValid)
                {
                    return Results.ValidationProblem(result.GetValidationProblems());
                }

                _unitOfWork.BikeRepository.Add(request.Bike);

                request.Bike.DomainEvents.Add(new BikeCreateEvent(request.Bike,
                    $"The bike {request.Bike.Model} {request.Bike.Brand} - {request.Bike.Type} has been created succesfully."));

                if (await _unitOfWork.Complete())
                    return Results.Ok(request);

                return Results.BadRequest("Problem creating the user");
            }
        }        
    }
}
