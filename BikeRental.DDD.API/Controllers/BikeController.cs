using AutoMapper;
using BikeRental.DDD.Application.Commands;
using BikeRental.DDD.Application.Extensions;
using BikeRental.DDD.Domain;
using BikeRental.DDD.Domain.Entities;
using BikeRental.DDD.Domain.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using BikeRental.DDD.Domain.IServices;
using BikeRental.DDD.Infrastructure.Services;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using BikeRental.DDD.Domain.DTOs.User;
using BikeRental.DDD.Domain.DTOs.Photo;
using BikeRental.DDD.Application.Queries;
using BikeRental.DDD.Application.Queries.User;
using static BikeRental.DDD.Application.Queries.User.GetUserByUsername;
using BikeRental.DDD.Domain.DTOs.Bike;
using BikeRental.DDD.Application.Queries.Bike;
using BikeRental.DDD.Application.Commands.Bike;

namespace BikeRental.DDD.API.Controllers
{
    [Route("api/[controller]")]
    //[ServiceFilter(typeof(LogUserActivity))]
    [ApiController]    
    public class BikeController : ControllerBase
    {
        private readonly ILogger<BikeController> _logger;
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        public BikeController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<BikeController> logger, IMediator mediator, IPhotoService photoService)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _photoService = photoService;
        }

        [HttpGet("bikes")]
        public async Task<ActionResult<PagedList<BikeDTO>>> GetBikesByParams([FromQuery] BikeParams bikeParams)
        {           
            var bikes = await _unitOfWork.BikeRepository.GetBikesAsync(bikeParams);

            Response.AddPaginationHeader(new PaginationHeader(bikes.CurrentPage, bikes.PageSize,
                bikes.TotalCount, bikes.TotalPages));

            return Ok(bikes);
        } 

        /// <summary>
        /// Obtiene el listado de usuarios, usando el patrón CQRS(GetUsersQuery).
        /// Comprueba si algunos de los usuarios obtenidos se encuentra expirado y lo desactiva con borrado lógico.
        /// </summary>
        /// <returns></returns>
        [HttpGet("list")]
        public IActionResult GetBikes([FromQuery] BikeParams bikeParams)
        {
            var response = _mediator.Send(new GetBikes.GetBikesQuery(bikeParams)).GetAwaiter().GetResult();
            var users = response;
          
            return Ok(users);
        }
        
        [HttpGet("{id}")]
        public IActionResult GetBikeById(int id)
        {            
            var bike = _mediator.Send(new GetBikeById.ByIdQuery(id)).GetAwaiter().GetResult();

            return bike is null ? NotFound() : Ok(bike);
        }

        [HttpPost]
        public async Task<ActionResult> CreateBike(Bike bike)
        {
            var bikeCreated = await _mediator.Send(new CreateBike.CreateBikeCommand(bike));

            return Ok(bikeCreated);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateBike(BikeDTO bikeDTO)
        {            
            var bikeUpdated = await _mediator.Send(new UpdateBike.UpdateBikeCommand(bikeDTO));

            return Ok(bikeUpdated);
        }

        [HttpPost("add-photo/{bikeId}")]
        public async Task<ActionResult<PhotoDTO>> AddPhoto([FromForm] IFormFile file, [FromRoute] int bikeId)
        {                               
            var photoInserted = _mediator.Send(new CreateBikePhoto.CreatePhotoCommand(bikeId, file)).GetAwaiter().GetResult();
            return Ok(photoInserted);            
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {                        
            var result = await _mediator.Send(new UpdateBikePhoto.SetMainPhotoCommand(photoId));
            return Ok(result);
        }

        [HttpDelete("delete-photo/{bikeId}/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int bikeId, int photoId)
        {                        
            await _mediator.Send(new DeleteBikePhoto.DeletePhotoCommand(bikeId, photoId));

            return Ok();
        }

        /// <summary>
        /// Elmina un usuario por el nombre enviado por parámetro, usando el patrón CQRS(DeleteUserCommand).
        /// </summary>
        /// <param name="name">Nombre del usuario</param>
        /// <returns>Devuelve el usuario eliminado</returns>
        [HttpDelete("remove-bike/{id}")]
        public IActionResult DeleteBikeByName(int id)
        {
            var response = _mediator.Send(new DeleteBike.DeleteBikeCommand(id)).GetAwaiter().GetResult();            

            return Ok(response);
        }        

        /// <summary>
        /// Elimina la entidad User de la bbddd
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpDelete("remove/{id}")]
        public async Task<ActionResult> RemoveBike(int id)
        {
            var bike = await _unitOfWork.BikeRepository.GetBikeByIdAsync(id);
            _unitOfWork.BikeRepository.Remove(bike);

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Problem removing the bike");
        }

        /// <summary>
        /// Filtrado de usuarios.
        /// </summary>
        /// <param name="userParams">Parámetros de búsqueda para la entidad User</param>
        /// <returns>Devuelve el listado de usuarios encontrados paginados.</returns>
        [HttpGet("bikes-by-params")]
        public async Task<ActionResult<IEnumerable<BikeDTO>>> GetBikeList([FromQuery] BikeParams bikeParams)
        {
            var bikes = await _unitOfWork.BikeRepository.GetBikesAsync(bikeParams);

            Response.AddPaginationHeader(new PaginationHeader(bikes.CurrentPage, bikes.PageSize,
                bikes.TotalCount, bikes.TotalPages));

            return Ok(bikes);
        }

        /// <summary>
        /// Filtrado de usuarios. 
        /// </summary>
        /// <param name="textFilter">Parámetros de búsqueda para la entidad User</param>
        /// <returns>Devuelve el listado de usuarios encontrados paginados.</returns>
        [HttpGet("filter-bikes-by-text/{textFilter}")]
        public async Task<ActionResult<IEnumerable<Bike>>> GetBikeList(string textFilter)
        {
            var bikeParams = new BikeParams();
            bikeParams.Model = textFilter;
            bikeParams.Brand = textFilter;
            bikeParams.Type = textFilter;
            bikeParams.Year = int.Parse(textFilter);            

            var bikes = await _unitOfWork.BikeRepository.GetBikesByTextFilterAsync(bikeParams);

            Response.AddPaginationHeader(new PaginationHeader(bikes.CurrentPage, bikes.PageSize,
                bikes.TotalCount, bikes.TotalPages));

            return Ok(bikes);
        }
    }
}
