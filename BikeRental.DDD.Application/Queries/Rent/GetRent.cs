using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BikeRental.DDD.Domain;
using BikeRental.DDD.Domain.DTOs.Bike;
using BikeRental.DDD.Domain.DTOs.Rental;
using BikeRental.DDD.Domain.DTOs.User;
using BikeRental.DDD.Domain.Entities;
using BikeRental.DDD.Infrastructure;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace BikeRental.DDD.Application.Queries
{
    /// <summary>
    /// Queries for retrieving data following the CQRS pattern
    /// </summary>
    public class GetRent
    {                
        public record GetRentalsQuery(int BikeId) : IRequest<BikeRentalHistoryDTO>;

        // Handler        
        public class Handler : IRequestHandler<GetRentalsQuery, BikeRentalHistoryDTO>
        {
            private readonly DataContext _context;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IUnitOfWork unitOfWork, IMapper mapper)
            {
                _context = context;
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task<BikeRentalHistoryDTO> Handle(GetRentalsQuery request, CancellationToken cancellationToken)
            {
                var bike = await _unitOfWork.BikeRepository.GetBikeAsync(request.BikeId);
                if (bike == null) return null;

                var rentals = await _unitOfWork.RentalRepository.GetRentalsByBike(request.BikeId);
                var rentalsByUser = rentals.ToLookup(r => r.CustomerId);

                return new BikeRentalHistoryDTO
                {
                    Bike = new BikeDTO
                    {
                        Id = bike.Id,
                        Model = bike.Model,
                        Brand = bike.Brand,
                        Price = bike.Price,
                        PhotoUrl = bike.BikePhotos.FirstOrDefault(p => p.IsMain)?.Url                       
                    },                    
                    Rentals = rentalsByUser.Select(group =>
                    {
                        var rental = group.OrderByDescending(r => r.StartDate).First();
                        return new RentalHistoryCustomerDTO
                        {
                            UserId = rental.CustomerId,
                            Name = rental.Customer.Name,
                            Surname = rental.Customer.Surname,
                            Username = rental.Customer.UserName,
                            PhotoUrl = rental.Customer.UserPhotos.FirstOrDefault(p => p.IsMain)?.Url,
                            StartDate = rental.StartDate,
                            EndDate = rental.EndDate
                        };
                    }).ToList()

                };
            }
        }        
    }
}
