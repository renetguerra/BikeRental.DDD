using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BikeRental.DDD.Domain.Entities;

namespace BikeRental.DDD.Application.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<User> FindUserByClaimsPrincipleWithAddress(this UserManager<User> userManager,
            ClaimsPrincipal user)
        {
            var email = user.FindFirstValue(ClaimTypes.Email);

            return await userManager.Users //.Include(x => x.Address)
                .SingleOrDefaultAsync(x => x.Email == email);
        }

        public static async Task<User> FindByEmailFromClaimsPrincipal(this UserManager<User> userManager,
            ClaimsPrincipal user)
        {
            return await userManager.Users
                .SingleOrDefaultAsync(x => x.Email == user.FindFirstValue(ClaimTypes.Email));
        }
    }
}