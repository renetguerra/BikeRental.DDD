using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikeRental.DDD.Domain.DTOs.Rental;
using BikeRental.DDD.Domain.DTOs.User;
using BikeRental.DDD.Domain;
using MediatR;
using BikeRental.DDD.Domain.Entities;
using Microsoft.AspNetCore.Routing;
using Carter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BikeRental.DDD.Application.Queries.Rent
{
    public class GetCustomerRentalHistory
    {        
        public record GetCustomerRentalHistoryQuery(string Username) : IRequest<CustomerRentalHistoryDTO>;

        // Handler
        public class GetCustomerRentalHistoryHandler : IRequestHandler<GetCustomerRentalHistoryQuery, CustomerRentalHistoryDTO>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetCustomerRentalHistoryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }
            public async Task<CustomerRentalHistoryDTO> Handle(GetCustomerRentalHistoryQuery request, CancellationToken cancellationToken)
            {
                var customer = await _unitOfWork.UserRepository.GetUserByUsernameAsync(request.Username);
                if (customer == null) return null;

                var rentals = await _unitOfWork.RentalRepository.GetRentalsByCustomer(customer.Id);

                return new CustomerRentalHistoryDTO
                {
                    Customer = new UserDTO
                    {
                        Username = customer.UserName,
                        KnownAs = customer.KnownAs,
                        PhotoUrl = customer.UserPhotos.FirstOrDefault(p => p.IsMain)?.Url,
                        Email = customer.Email
                    },
                    Rentals = rentals.Select(r => new RentalHistoryDTO
                    {
                        BikeId = r.BikeId,
                        ModelBike = r.Bike.Model,
                        BrandBike = r.Bike.Brand,
                        PriceBike = r.Bike.Price,
                        PhotoUrl = r.Bike.BikePhotos.FirstOrDefault(p => p.IsMain)?.Url,
                        StartDate = r.StartDate,
                        EndDate = r.EndDate
                    }).ToList()
                };
            }
        }
    }
}
