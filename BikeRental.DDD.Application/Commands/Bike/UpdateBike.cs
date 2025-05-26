using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BikeRental.DDD.Application.Extensions;
using BikeRental.DDD.Domain;
using BikeRental.DDD.Domain.DTOs.Bike;
using BikeRental.DDD.Domain.Entities;
using BikeRental.DDD.Domain.Events;
using BikeRental.DDD.Domain.Helpers;
using BikeRental.DDD.Infrastructure;
using Carter;
using Carter.ModelBinding;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace BikeRental.DDD.Application.Commands
{
    public class UpdateBike
    {        
        public record UpdateBikeCommand(BikeDTO BikeDTO) : IRequest<IResult>;
        
        public class UpdateBikeHandler : IRequestHandler<UpdateBikeCommand, IResult>
        {
            private readonly IValidator<BikeRental.DDD.Domain.Entities.Bike> _validator;
            private readonly DataContext _context;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public UpdateBikeHandler(DataContext context, IUnitOfWork unitOfWork, IMapper mapper, IValidator<BikeRental.DDD.Domain.Entities.Bike> validator)
            {
                _context = context;
                _unitOfWork = unitOfWork;
                _validator = validator;
                _mapper = mapper;
            }
            public async Task<IResult> Handle(UpdateBikeCommand request, CancellationToken cancellationToken)
            {
                var bike = await _unitOfWork.BikeRepository.GetBikeByIdAsync(request.BikeDTO.Id);
                if (bike is null) return Results.NotFound();

                _mapper.Map(request.BikeDTO, bike);

                var validationResult = _validator.Validate(bike);
                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.GetValidationProblems());
                }

                bike.DomainEvents.Add(new BikeUpdateEvent(bike, "The bike has been updated."));
                _unitOfWork.BikeRepository.Update(bike);
               

                if (await _unitOfWork.Complete())
                    return Results.Ok(request);

                return Results.BadRequest("Problem updating the bike");
            }            

        }
    }
}
