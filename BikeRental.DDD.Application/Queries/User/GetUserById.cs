using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikeRental.DDD.Domain;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BikeRental.DDD.Application.Queries.User
{
    public class GetUserById
    {              
        public record ByIdQuery(int Id) : IRequest<BikeRental.DDD.Domain.Entities.User?>;
        
        public class ByIdHandler : IRequestHandler<ByIdQuery, BikeRental.DDD.Domain.Entities.User?>
        {
            private readonly IUnitOfWork _unitOfWork;

            public ByIdHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }
            public async Task<BikeRental.DDD.Domain.Entities.User?> Handle(ByIdQuery request, CancellationToken cancellationToken)
            {
                var user = await _unitOfWork.UserRepository.GetUserByIdAsync(request.Id);
                return user;                
            }
        }
    }
}
