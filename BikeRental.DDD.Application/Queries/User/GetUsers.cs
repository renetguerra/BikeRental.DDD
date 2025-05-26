using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Azure;
using BikeRental.DDD.Domain;
using BikeRental.DDD.Domain.DTOs.User;
using BikeRental.DDD.Domain.Entities;
using BikeRental.DDD.Infrastructure;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace BikeRental.DDD.Application.Queries
{
    /// <summary>
    /// Queries for retrieving data following the CQRS pattern
    /// </summary>
    public class GetUsers
    {                
        public record GetUsersQuery() : IRequest<IEnumerable<UserDTO>>;
        
        public class Handler : IRequestHandler<GetUsersQuery, IEnumerable<UserDTO>>
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

            public async Task<IEnumerable<UserDTO>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
            {
                var users = await _unitOfWork.UserRepository.GetUsersAsync();                
                return _mapper.Map<IEnumerable<UserDTO>>(users);
            }
        }       
    }
}
