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
    public class GetLikedBikeIdsByCustomer
    {        
        public record GetLikedBikeIdsByCustomerQuery(string Username) : IRequest<IEnumerable<int>>;

        // Handler
        public class Handler : IRequestHandler<GetLikedBikeIdsByCustomerQuery, IEnumerable<int>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }
            public async Task<IEnumerable<int>> Handle(GetLikedBikeIdsByCustomerQuery request, CancellationToken cancellationToken)
            {
                var customer = await _unitOfWork.UserRepository.GetUserByUsernameAsync(request.Username);
                var rent = await _unitOfWork.LikesRepository.GetCurrentUserLikeBikeIds(customer.Id);
                return rent;
            }
        }

    }
}
