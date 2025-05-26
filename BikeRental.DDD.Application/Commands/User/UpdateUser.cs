using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BikeRental.DDD.Application.Extensions;
using BikeRental.DDD.Domain;
using BikeRental.DDD.Domain.DTOs.User;
using BikeRental.DDD.Domain.Entities;
using BikeRental.DDD.Domain.Events;
using BikeRental.DDD.Domain.Helpers;
using BikeRental.DDD.Infrastructure;
using Carter;
using Carter.ModelBinding;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace BikeRental.DDD.Application.Commands
{
    public class UpdateUser
    {        
        public record UpdateUserCommand(CustomerUpdateDTO CustomerUpdateDTO) : IRequest<IResult>;

        // Handler
        public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, IResult>
        {
            private readonly IValidator<User> _validator;
            private readonly DataContext _context;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public UpdateUserHandler(DataContext context, IUnitOfWork unitOfWork, IMapper mapper, IValidator<User> validator)
            {
                _context = context;
                _unitOfWork = unitOfWork;
                _validator = validator;
                _mapper = mapper;
            }
            public async Task<IResult> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
            {
                var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(request.CustomerUpdateDTO.UserName);
                if (user is null) return Results.NotFound();

                _mapper.Map(request.CustomerUpdateDTO, user);

                var validationResult = _validator.Validate(user);
                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.GetValidationProblems());
                }

                user.DomainEvents.Add(new UserUpdateEvent(user, "The user has been updated."));
                _unitOfWork.UserRepository.Update(user);


                if (await _unitOfWork.Complete())
                    return Results.Ok(request);

                return Results.BadRequest("Problem updating the user");
            }            

        }
    }
}
