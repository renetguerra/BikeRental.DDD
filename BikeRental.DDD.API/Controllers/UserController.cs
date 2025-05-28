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

namespace BikeRental.DDD.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[ServiceFilter(typeof(LogUserActivity))]
    //[Authorize]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        public UserController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UserController> logger, IMediator mediator, IPhotoService photoService)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _photoService = photoService;
        }

        [HttpGet("users")]
        public async Task<ActionResult<PagedList<CustomerDTO>>> GetUsers([FromQuery] UserParams userParams)
        {
           var currentUser = await _unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());
           userParams.CurrentUsername = currentUser.UserName; 

            if (string.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender = currentUser.Gender == "male" ? "female" : "male";
            }

            var users = await _unitOfWork.UserRepository.GetMembersAsync(userParams);

            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize,
                users.TotalCount, users.TotalPages));

            return Ok(users);
        } 

        /// <summary>
        /// Obtiene el listado de usuarios, usando el patrón CQRS(GetUsersQuery).
        /// Comprueba si algunos de los usuarios obtenidos se encuentra expirado y lo desactiva con borrado lógico.
        /// </summary>
        /// <returns></returns>
        [HttpGet("list")]
        public IActionResult GetUsers()
        {
            var response = _mediator.Send(new GetUsers.GetUsersQuery()).GetAwaiter().GetResult();
            var users = response;
          
            return Ok(users);
        }

        /// <summary>
        /// Obtiene un usuario mediante su Identificador, usando el patrón CQRS(ByIdQuery).
        /// </summary>
        /// <param name="id">Identificador del usuario</param>
        /// <returns></returns>
        //[HttpGet("{id}")]
        //public IActionResult GetUserById(int id)
        //{
        //    var response = _mediator.Send(new GetUsers.ByIdQuery(id)).GetAwaiter().GetResult();
        //    var user = response;

        //    return Ok(user);          
        //}

        /// <summary>
        /// Obtiene un usuario mediante su Username, usando el patrón CQRS(ByUsernameQuery).
        /// </summary>
        /// <param name="username">Username del usuario</param>
        /// <returns></returns>
        [HttpGet("{username}")]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            var currentUsername = User.GetUsername();
            var response = await _mediator.Send(new GetUserByUsernameQuery(username, currentUsername == username));        

            return Ok(response);            
        }


        [HttpPut]
        public async Task<ActionResult> UpdateUser(CustomerUpdateDTO memberUpdateDTO)
        {                        
            var userUpdated = _mediator.Send(new UpdateUser.UpdateUserCommand(memberUpdateDTO)).GetAwaiter().GetResult();
            
            return Ok(userUpdated);
        }

        [HttpPost("add-photo/{userName}")]
        public async Task<ActionResult<PhotoDTO>> AddPhoto(IFormFile file, string userName)
        {                       
            var photoInserted = _mediator.Send(new CreateUserPhoto.CreatePhotoCommand(userName, file)).GetAwaiter().GetResult();
            return Ok(photoInserted);            
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var result = await _mediator.Send(new UpdateUserPhoto.SetMainPhotoCommand(photoId));
            return Ok(result);
        }

        [HttpDelete("delete-photo/{username}/{photoId}")]
        public async Task<ActionResult> DeletePhoto(string username, int photoId)
        {
            _mediator.Send(new DeleteUserPhoto.DeletePhotoCommand(username, photoId)).GetAwaiter().GetResult();

            return Ok();
        }

        /// <summary>
        /// Elmina un usuario por el nombre enviado por parámetro, usando el patrón CQRS(DeleteUserCommand).
        /// </summary>
        /// <param name="name">Nombre del usuario</param>
        /// <returns>Devuelve el usuario eliminado</returns>
        [HttpDelete("remove-user-name/{name}")]
        public IActionResult DeleteUserByName(string name)
        {
            var response = _mediator.Send(new DeleteUser.DeleteUserCommand(name)).GetAwaiter().GetResult();
            var userDeleted = response;

            return Ok(userDeleted);
        }        

        /// <summary>
        /// Elimina la entidad User de la bbddd
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpDelete("remove-user/{id}")]
        public async Task<ActionResult> RemoveUser(int id)
        {
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(id);
            _unitOfWork.UserRepository.RemoveUser(user);

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Problem removing the user");
        }

        /// <summary>
        /// Filtrado de usuarios.
        /// </summary>
        /// <param name="userParams">Parámetros de búsqueda para la entidad User</param>
        /// <returns>Devuelve el listado de usuarios encontrados paginados.</returns>
        [HttpGet("users-by-params")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUserList([FromQuery] UserParams userParams)
        {
            var users = await _unitOfWork.UserRepository.GetMembersAsync(userParams);

            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize,
                users.TotalCount, users.TotalPages));

            return Ok(users);
        }

        /// <summary>
        /// Filtrado de usuarios. 
        /// </summary>
        /// <param name="textFilter">Parámetros de búsqueda para la entidad User</param>
        /// <returns>Devuelve el listado de usuarios encontrados paginados.</returns>
        [HttpGet("filter-users-by-text/{textFilter}")]
        public async Task<ActionResult<IEnumerable<User>>> GetUserList(string textFilter)
        {
            var userParams = new UserParams();
            userParams.Name = textFilter;
            userParams.Surname = textFilter;
            userParams.Username = textFilter;
            userParams.KnownAs = textFilter;
            userParams.Email = textFilter;
            userParams.PhoneNumber = textFilter;

            var users = await _unitOfWork.UserRepository.GetUsersByTextFilterAsync(userParams);

            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize,
                users.TotalCount, users.TotalPages));

            return Ok(users);
        }
    }
}
