using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikeRental.DDD.Application.Commands;
using BikeRental.DDD.Domain;
using BikeRental.DDD.Domain.Entities;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

namespace BikeRental.Tests.Commands.Rent
{
    public class RentBike_CreateRentCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUow;
        private readonly Mock<IValidator<Rental>> _mockValidator;
        private readonly DDD.Application.Commands.Rent.CreateRentalHandler _handler;

        public RentBike_CreateRentCommandHandlerTests()
        {
            _mockUow = new Mock<IUnitOfWork>();
            _mockValidator = new Mock<IValidator<Rental>>();
            _handler = new DDD.Application.Commands.Rent.CreateRentalHandler(_mockUow.Object, _mockValidator.Object);
        }

        [Fact]
        public async Task Handle_ValidRental_ShouldRentBike()
        {
            // Arrange
            var rental = new Rental { BikeId = 1, CustomerId = 1 };
            var command = new DDD.Application.Commands.Rent.CreateRentCommand(rental.BikeId, rental.CustomerId);

            _mockValidator.Setup(v => v.Validate(rental))
                          .Returns(new ValidationResult());

            _mockUow.Setup(x => x.BikeRepository.GetBikeByIdAsync(1))
                    .ReturnsAsync(new Bike { Id = 1, IsAvailable = true });

            _mockUow.Setup(x => x.UserRepository.GetUserByIdAsync(1))
                    .ReturnsAsync(new User { Id = 1 });

            _mockUow.Setup(x => x.RentalRepository.GetActiveRental(1, 1))
                    .ReturnsAsync((Rental)null);

            _mockUow.Setup(x => x.Complete()).ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeAssignableTo<IResult>();

            var okResult = result as Ok<DDD.Application.Commands.Rent.CreateRentCommand>;
            okResult!.Value.Should().Be(command);

        }
    }
}
