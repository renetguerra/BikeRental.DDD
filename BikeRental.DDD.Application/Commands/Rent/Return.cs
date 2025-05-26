using BikeRental.DDD.Domain;
using BikeRental.DDD.Domain.Entities;
using BikeRental.DDD.Domain.Events.Rent;
using Carter;
using Carter.ModelBinding;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace BikeRental.DDD.Application.Commands
{
    public class Return
    {       
        public record UpdateRentCommand(int BikeId, int CustomerId) : IRequest<IResult>;

        public class UpdateRentalHandler : IRequestHandler<UpdateRentCommand, IResult>
        {
            private readonly IValidator<Rental> _validator;
            private readonly IUnitOfWork _unitOfWork;

            public UpdateRentalHandler(IUnitOfWork unitOfWork, IValidator<Rental> validator)
            {
                _unitOfWork = unitOfWork;
                _validator = validator;
            }

            public async Task<IResult> Handle(UpdateRentCommand request, CancellationToken cancellationToken)
            {
                var bike = await _unitOfWork.BikeRepository.GetBikeByIdAsync(request.BikeId);
                if (bike is not null && bike.IsAvailable)
                    return Results.BadRequest("This bike is available");

                var rental = await _unitOfWork.RentalRepository
                .GetActiveRental(request.BikeId, request.CustomerId);

                if (rental is null)
                    return Results.NotFound("Rental not found");

                if (rental.EndDate is not null || rental.IsReturned)
                    return Results.BadRequest("Bike already returned");

                rental.EndDate = DateTime.UtcNow;
                
                bike.IsAvailable = true;
                _unitOfWork.BikeRepository.Update(bike);

                rental.DomainEvents.Add(new RentEvent(rental, "You returned the bike."));

                _unitOfWork.RentalRepository.Update(rental);

                if (await _unitOfWork.Complete())
                    return Results.Ok(rental);

                return Results.BadRequest("Problem returning the bike");                
            }
        }
    }
}
