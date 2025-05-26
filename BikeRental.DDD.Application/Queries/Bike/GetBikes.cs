using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BikeRental.DDD.Domain;
using BikeRental.DDD.Domain.DTOs.Bike;
using BikeRental.DDD.Domain.DTOs.Like;
using BikeRental.DDD.Domain.Entities;
using BikeRental.DDD.Domain.Helpers;
using BikeRental.DDD.Infrastructure;
using Carter;
using CloudinaryDotNet.Actions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace BikeRental.DDD.Application.Queries.Bike
{
    /// <summary>
    /// Queries for retrieving data following the CQRS pattern
    /// </summary>
    public class GetBikes
    {             
        public record GetBikesQuery(BikeParams BikeParams) : IRequest<PagedList<BikeDTO>>;
        
        public class Handler : IRequestHandler<GetBikesQuery, PagedList<BikeDTO>>
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
            
            public async Task<PagedList<BikeDTO>> Handle(GetBikesQuery request, CancellationToken cancellationToken)
            {
                var bikes = await _unitOfWork.BikeRepository.GetBikesAsync(request.BikeParams);
                return bikes;
            }
        }                   

    }
}
