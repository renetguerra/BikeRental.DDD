using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BikeRental.DDD.Application.Extensions;
using BikeRental.DDD.Domain.Entities;
using BikeRental.DDD.Domain.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Google.Apis.Auth;
using BikeRental.DDD.Domain;
using Newtonsoft.Json;
using Azure.Core;
using System.Net.Http;
using CloudinaryDotNet;
using Azure;
using BikeRental.DDD.Domain.DTOs.User;

namespace BikeRental.DDD.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[AllowAnonymous]
    public class AccountController(UserManager<User> userManager, ITokenService tokenService,
    IMapper mapper) : BaseApiController
    {
        [HttpPost("register")] // account/register
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            if (await UserExists(registerDTO.Username)) return BadRequest("Username is taken");

            var user = mapper.Map<User>(registerDTO);

            user.UserName = registerDTO.Username.ToLower();

            var result = await userManager.CreateAsync(user, registerDTO.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            return new UserDTO
            {
                Username = user.UserName,
                Token = await tokenService.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var user = await userManager.Users
                .Include(p => p.UserPhotos)
                    .FirstOrDefaultAsync(x =>
                        x.NormalizedUserName == loginDTO.Username.ToUpper());

            if (user == null || user.UserName == null) return Unauthorized("Invalid username");

            return new UserDTO
            {
                Username = user.UserName,
                KnownAs = user.KnownAs,
                Token = await tokenService.CreateToken(user),
                Gender = user.Gender,
                PhotoUrl = user.UserPhotos.FirstOrDefault(x => x.IsMain)?.Url,                
            };
        }

        private async Task<bool> UserExists(string username)
        {
            return await userManager.Users.AnyAsync(x => x.NormalizedUserName == username.ToUpper()); // Bob != bob
        }
    }
}