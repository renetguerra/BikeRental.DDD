using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BikeRental.DDD.Domain;
using BikeRental.DDD.Domain.DTOs.Bike;
using BikeRental.DDD.Domain.DTOs.Like;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BikeRental.DDD.Application.Queries.User
{
    public class GetBikeById
    {              
        public record ByIdQuery(int Id) : IRequest<BikeDTO>;
        
        public class ByIdHandler : IRequestHandler<ByIdQuery, BikeDTO>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public ByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }
            public async Task<BikeDTO?> Handle(ByIdQuery request, CancellationToken cancellationToken)
            {
                var bike = await _unitOfWork.BikeRepository.GetBikeAsync(request.Id);
                return bike is null ? null : _mapper.Map<BikeDTO>(bike);
            }
        }
    }
}
