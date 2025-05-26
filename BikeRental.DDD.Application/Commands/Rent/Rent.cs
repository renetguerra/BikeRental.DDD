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
    public class Rent
    {        
        public record CreateRentCommand(int BikeId, int CustomerId) : IRequest<IResult>;

        public class CreateRentalHandler : IRequestHandler<CreateRentCommand, IResult>
        {
            private readonly IValidator<Rental> _validator;
            private readonly IUnitOfWork _unitOfWork;

            public CreateRentalHandler(IUnitOfWork unitOfWork, IValidator<Rental> validator)
            {
                _unitOfWork = unitOfWork;
                _validator = validator;
            }

            public async Task<IResult> Handle(CreateRentCommand request, CancellationToken cancellationToken)
            {
                var bike = await _unitOfWork.BikeRepository.GetBikeByIdAsync(request.BikeId);
                if (bike is null || !bike.IsAvailable)
                    return Results.BadRequest("Bike not available");

                var customer = await _unitOfWork.UserRepository.GetUserByIdAsync(request.CustomerId);
                if (customer is null)
                    return Results.BadRequest("Customer not found");

                var existingRental = await _unitOfWork.RentalRepository
                .GetActiveRental(request.BikeId, request.CustomerId);

                if (existingRental is not null)
                    return Results.BadRequest("Customer already has this bike rented");

                var rental = new Rental
                {
                    BikeId = request.BikeId,
                    CustomerId = request.CustomerId,
                    StartDate = DateTime.UtcNow
                };

                var validationResult = _validator.Validate(rental);
                if (!validationResult.IsValid)
                    return Results.ValidationProblem(validationResult.GetValidationProblems());

                rental.DomainEvents.Add(new RentEvent(rental, "You rented this bike."));

                _unitOfWork.RentalRepository.RentBike(rental);

                bike.IsAvailable = false;
                _unitOfWork.BikeRepository.Update(bike);

                if (await _unitOfWork.Complete())
                    return Results.Ok(rental);

                return Results.BadRequest("Problem renting the bike");
            }
        }
    }
}
