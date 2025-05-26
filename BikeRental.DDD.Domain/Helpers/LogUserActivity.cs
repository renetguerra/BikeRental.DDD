using System;
using System.Security.Claims;
using System.Threading.Tasks;
using BikeRental.DDD.Domain.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using BikeRental.DDD.Domain.Entities;

namespace BikeRental.DDD.Domain.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();

            if (!resultContext.HttpContext.User.Identity.IsAuthenticated) return;

            var userId = resultContext.HttpContext.User.GetUserId();
            var userName = resultContext.HttpContext.User.GetUsername();

            var uow = resultContext.HttpContext.RequestServices.GetRequiredService<IUnitOfWork>();
            //var user = await uow.UserRepository.GetUserByIdAsync(userId);
            var user = await uow.UserRepository.GetUserByUsernameAsync(userName);
            user.LastActive = DateTime.UtcNow;
            await uow.Complete();
        }
    }
}