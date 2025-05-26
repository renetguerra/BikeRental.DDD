using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BikeRental.DDD.Domain.DTOs.Like;
using BikeRental.DDD.Domain.Entities;
using BikeRental.DDD.Domain.Helpers;
using BikeRental.DDD.Domain;
using BikeRental.DDD.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Routing;
using Carter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace BikeRental.DDD.Application.Queries.Rent
{
    public class GetByBikeCustomer
    {        
        public record GetByBikeCustomerIdQuery(int BikeId, string Username) : IRequest<Rental?>;

        // Handler
        public class ByBikeCustomerIdHandler : IRequestHandler<GetByBikeCustomerIdQuery, Rental?>
        {
            private readonly IUnitOfWork _unitOfWork;

            public ByBikeCustomerIdHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }
            public async Task<Rental?> Handle(GetByBikeCustomerIdQuery request, CancellationToken cancellationToken)
            {
                var customer = await _unitOfWork.UserRepository.GetUserByUsernameAsync(request.Username);
                var rent = await _unitOfWork.RentalRepository.GetActiveRental(request.BikeId, customer.Id);
                return rent;
            }
        }

    }
}
