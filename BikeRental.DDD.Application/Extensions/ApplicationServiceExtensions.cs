using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BikeRental.DDD.Application.Security;
using BikeRental.DDD.Domain;
using BikeRental.DDD.Domain.Entities;
using BikeRental.DDD.Domain.Helpers;
using BikeRental.DDD.Domain.IRepositories;
using BikeRental.DDD.Domain.Validators;
using BikeRental.DDD.Infrastructure;
using BikeRental.DDD.Infrastructure.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using BikeRental.DDD.Domain.IServices;
using BikeRental.DDD.Infrastructure.Services;
using BikeRental.DDD.Infrastructure.SignalR;
using BikeRental.DDD.Application.Helpers;
using Microsoft.AspNetCore.Mvc;
using BikeRental.DDD.Domain.Errors;
using StackExchange.Redis;
using System.Text.Json.Serialization;

namespace BikeRental.DDD.Application.Extensions
{
    /// <summary>
    /// Custom class for service configurations, dependency injections,
    /// database connections, etc.
    /// </summary>
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IBikeRepository, BikeRepository>();
            services.AddScoped<ILikesRepository, LikesRepository>();
            services.AddScoped<IConnectionRepository, ConnectionRepository>();            
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPhotoRepository, PhotoRepository>();
            services.AddScoped<IRentalRepository, RentalRepository>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<LogUserActivity>();

            //services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);            
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));

            services.AddSignalR();

            services.AddSingleton<PresenceTracker>();
            services.AddSingleton<IResponseCacheService, ResponseCacheService>();
            services.AddSingleton<IConnectionMultiplexer>(c =>
            {
                var options = ConfigurationOptions.Parse(config.GetConnectionString("Redis"));
                return ConnectionMultiplexer.Connect(options);
            });

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(Application).Assembly);
            });

            services.AddValidatorsFromAssemblyContaining(typeof(Application));
            services.AddTransient<IValidator<User>, UserValidator>();
            services.AddTransient<IValidator<Bike>, BikeValidator>();
            services.AddTransient<IValidator<BikePhoto>, BikePhotoValidator>();
            services.AddTransient<IValidator<UserPhoto>, UserPhotoValidator>();
            services.AddTransient<IValidator<Like>, LikeValidator>();
            services.AddTransient<IValidator<Rental>, RentalValidator>();

            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .SelectMany(x => x.Value.Errors)
                        .Select(x => x.ErrorMessage).ToArray();

                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            });

            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("https://localhost:4200");
                });
            });

            return services;
        }
    }
}
