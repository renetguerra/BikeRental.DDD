using BikeRental.DDD.Application.Extensions;
using BikeRental.DDD.Domain;
using BikeRental.DDD.Domain.Entities;
using BikeRental.DDD.Domain.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using AutoMapper;
using MediatR;
using BikeRental.DDD.Application.Commands;
using BikeRental.DDD.Domain.DTOs.Like;
using static BikeRental.DDD.Application.Queries.Likes.GetBikeLikes;
using static BikeRental.DDD.Application.Queries.Rent.GetByBikeCustomer;
using static BikeRental.DDD.Application.Queries.Rent.GetLikedBikeIdsByCustomer;

namespace BikeRental.DDD.API.Controllers
{
    [Route("api/[controller]")]
    //[ServiceFilter(typeof(LogUserActivity))]
    [ApiController]
    //[Authorize]
    public class LikesController(IUnitOfWork uow, IMapper mapper, ILogger<UserController> logger, IMediator mediator, IUnitOfWork unitOfWork) : ControllerBase
    {        
        [HttpPost("{bikeId}")]
        public async Task<ActionResult> ToogleLike(int bikeId)
        {
            var sourceUserId = User.GetUserId();                                                  

            var result = await mediator.Send(new ToogleLike.ToggleUserLikeCommand(sourceUserId, bikeId));

            return Ok(result);
        }        

        [HttpGet]
        public async Task<ActionResult<PagedList<LikeDTO>>> GetBikeLikes([FromQuery] LikesParams likesParams)
        {            
            var userId = User.GetUserId();
            var favoritBikes = await mediator.Send(new GetBikeLikesQuery(userId, likesParams));

            Response.AddPaginationHeader(new PaginationHeader(
                favoritBikes.CurrentPage,
                favoritBikes.PageSize,
                favoritBikes.TotalCount,
                favoritBikes.TotalPages));

            return Ok(favoritBikes);
        }
        
        [HttpGet("list/{username}")]
        public async Task<IActionResult> GetCurrentUserLikeBikeIds(string username)
        {
            var result = await mediator.Send(new GetLikedBikeIdsByCustomerQuery(username));
            return Ok(result);
        }
    }
}