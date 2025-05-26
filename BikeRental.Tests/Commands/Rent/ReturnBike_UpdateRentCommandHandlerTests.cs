using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikeRental.DDD.Application.Commands;
using BikeRental.DDD.Domain.Entities;
using BikeRental.DDD.Domain;
using FluentValidation;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using FluentValidation.Results;

namespace BikeRental.Tests.Commands.Rent
{
    public class ReturnBike_UpdateRentCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUow;
        private readonly Mock<IValidator<Rental>> _mockValidator;
        private readonly Return.UpdateRentalHandler _handler;

        public ReturnBike_UpdateRentCommandHandlerTests()
        {
            _mockUow = new Mock<IUnitOfWork>();
            _mockValidator = new Mock<IValidator<Rental>>();
            _handler = new Return.UpdateRentalHandler(_mockUow.Object, _mockValidator.Object);
        }

        [Fact]
        public async Task Handle_ValidReturn_ShouldUpdateRental()
        {
            // Arrange
            var rental = new Rental { BikeId = 1, CustomerId = 1 };
            var command = new Return.UpdateRentCommand(rental.BikeId, rental.CustomerId);

            _mockValidator.Setup(v => v.Validate(rental))
                          .Returns(new ValidationResult());

            var existingRental = new Rental { Id = Guid.NewGuid(), BikeId = 1, CustomerId = 1, StartDate = DateTime.UtcNow };
            _mockUow.Setup(x => x.RentalRepository.GetActiveRental(1, 1))
                    .ReturnsAsync(existingRental);

            _mockUow.Setup(x => x.Complete()).ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeOfType<Ok<Rental>>();
        }
    }
}
