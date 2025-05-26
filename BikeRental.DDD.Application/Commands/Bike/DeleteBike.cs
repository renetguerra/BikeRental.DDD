using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikeRental.DDD.Domain;
using BikeRental.DDD.Domain.Entities;
using BikeRental.DDD.Domain.Events;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace BikeRental.DDD.Application.Commands
{
    public class DeleteBike
    {        
        public record DeleteBikeCommand(int Id) : IRequest<IResult>;

        // Handler
        public class DeleteBikeHandler : IRequestHandler<DeleteBikeCommand, IResult>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMediator _mediator;

            public DeleteBikeHandler(IUnitOfWork unitOfWork, IMediator mediator)
            {
                _unitOfWork = unitOfWork;
                _mediator = mediator;
            }
            public async Task<IResult> Handle(DeleteBikeCommand request, CancellationToken cancellationToken)
            {
                var bike = await _unitOfWork.BikeRepository.GetBikeByIdAsync(request.Id);

                if (bike is null)
                {
                    return Results.NotFound();
                }
                
                bike.DomainEvents.Add(new BikeDeleteEvent(bike, $"The bike {bike.Model} - {bike.Brand} - {bike.Type} has been deleted"));

                _unitOfWork.BikeRepository.Remove(bike);

                if (await _unitOfWork.Complete())
                    return Results.Ok(bike);

                return Results.BadRequest("Problem removing the bike");
            }
        }
    }
}
