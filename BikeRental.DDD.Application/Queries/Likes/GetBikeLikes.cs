using BikeRental.DDD.Domain.Entities;
using BikeRental.DDD.Domain;
using MediatR;
using Microsoft.AspNetCore.Routing;
using Carter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using BikeRental.DDD.Domain.DTOs.Like;
using BikeRental.DDD.Domain.Helpers;

namespace BikeRental.DDD.Application.Queries.Likes
{
    public class GetBikeLikes
    {       
        public record GetBikeLikesQuery(int CustomerId, LikesParams Params) : IRequest<PagedList<LikeDTO>>;
       
        public class GetBikeLikesHandler : IRequestHandler<GetBikeLikesQuery, PagedList<LikeDTO>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetBikeLikesHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }
            public async Task<PagedList<LikeDTO>> Handle(GetBikeLikesQuery request, CancellationToken cancellationToken)
            {
                request.Params.UserId = request.CustomerId;

                return await _unitOfWork.LikesRepository.GetBikeLikes(request.Params);
            }
        }
    }
}
