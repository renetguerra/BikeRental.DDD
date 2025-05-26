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
using static BikeRental.DDD.Application.Queries.GetRent;
using static BikeRental.DDD.Application.Queries.Rent.GetCustomerRentalHistory;
using static BikeRental.DDD.Application.Queries.Rent.GetByBikeCustomer;

namespace BikeRental.DDD.API.Controllers
{
    [Route("api/[controller]")]
    //[ServiceFilter(typeof(LogUserActivity))]
    [ApiController]
    [Authorize]
    public class RentalController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;
        public RentalController(IUnitOfWork uow, IMapper mapper, ILogger<UserController> logger, IMediator mediator, IUnitOfWork unitOfWork)
        {
            _uow = uow;
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("{bikeId}")]
        public async Task<ActionResult> RentBike(int bikeId)
        {
            var customerId = User.GetUserId();           

            var result = await _mediator.Send(new Rent.CreateRentCommand(bikeId, customerId));

            return Ok(result);
        }

        [HttpPut("return/{bikeId}")]
        public async Task<ActionResult> ReturnBike(int bikeId)
        {
            var customerId = User.GetUserId();
            
            var result = await _mediator.Send(new Return.UpdateRentCommand(bikeId, customerId));

            return Ok(result);
        }

        [HttpGet("bike/{bikeId}")]
        public async Task<IActionResult> GetRentals(int bikeId)
        {
            var result = await _mediator.Send(new GetRentalsQuery(bikeId));
            return Ok(result);
        }

        [HttpGet("bike/{bikeId}/customer")]
        public async Task<IActionResult> GetRentalsByBikeCustomer(int bikeId, [FromQuery] string username)
        {
            var result = await _mediator.Send(new GetByBikeCustomerIdQuery(bikeId, username));
            return Ok(result);
        }

        [HttpGet("customer/{username}/history")]
        public async Task<IActionResult> GetCustomerRentalHistory(string username)
        {
            var result = await _mediator.Send(new GetCustomerRentalHistoryQuery(username));
            if (result == null) return NotFound();
            return Ok(result);
        }        
    }
}