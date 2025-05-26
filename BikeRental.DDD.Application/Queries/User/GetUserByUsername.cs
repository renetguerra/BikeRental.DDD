using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikeRental.DDD.Domain;
using BikeRental.DDD.Domain.DTOs.User;
using BikeRental.DDD.Domain.Helpers;
using BikeRental.DDD.Infrastructure;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace BikeRental.DDD.Application.Queries.User
{
    public class GetUserByUsername
    {                
        public record GetUserByUsernameQuery(string Username, bool IsCurrentUser) : IRequest<CustomerDTO?>;
        
        public class GetUserByUsernameHandler : IRequestHandler<GetUserByUsernameQuery, CustomerDTO?>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetUserByUsernameHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }           

            public async Task<CustomerDTO?> Handle(GetUserByUsernameQuery request, CancellationToken cancellationToken)
            {                
                var user = await _unitOfWork.UserRepository.GetCustomerAsync(request.Username, request.IsCurrentUser);                

                return user;
            }
        }
    }
}
