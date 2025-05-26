using BikeRental.DDD.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BikeRental.DDD.Domain.Helpers;
using BikeRental.DDD.Application.Extensions;
using BikeRental.DDD.Domain.DTOs.User;
using BikeRental.DDD.Infrastructure.Services;
using BikeRental.DDD.Infrastructure;
using AutoMapper;
using BikeRental.DDD.Domain.IServices;
using BikeRental.DDD.Domain;

namespace BikeRental.DDD.API.Controllers
{
    [Route("api/[controller]")]
    //[ServiceFilter(typeof(LogUserActivity))]
    [ApiController]
    [Authorize]
    public class AdminController(UserManager<User> userManager, IUnitOfWork unitOfWork, DataContext context, IMapper mapper,
    IPhotoService photoService) : ControllerBase
    {        
        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult<PagedList<UserRoleDTO>>> GetUsersWithRoles([FromQuery] UserRoleParams userRoleParams)
        {
            var query = userManager.Users
                .OrderBy(u => u.UserName)
                .AsQueryable();                

            var userRoles = query.Select(u => new UserRoleDTO
            {
                Id = u.Id,
                Username = u.UserName,
                Roles = u.UserRoles.Select(r => r.Role.Name).ToList()
            });

            var users = await PagedList<UserRoleDTO>.CreateAsync(userRoles, userRoleParams.PageNumber, userRoleParams.PageSize);

            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage,
                users.PageSize, users.TotalCount, users.TotalPages));

            return Ok(users);            
    }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("edit-roles/{username}")]
        public async Task<ActionResult> EditRoles(string username, [FromQuery]string roles)
        {
            if (string.IsNullOrEmpty(roles)) return BadRequest("You must select at least one role");

            var selectedRoles = roles.Split(",").ToArray();

            var user = await userManager.FindByNameAsync(username);

            if (user == null) return NotFound();

            var userRoles = await userManager.GetRolesAsync(user);

            var result = await userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if (!result.Succeeded) return BadRequest("Failed to add to roles");

            result = await userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if (!result.Succeeded) return BadRequest("Failed to remove from roles");

            return Ok(await userManager.GetRolesAsync(user));
        }    
    }
}