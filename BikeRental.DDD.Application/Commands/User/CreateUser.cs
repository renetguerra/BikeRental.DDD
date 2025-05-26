using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikeRental.DDD.Domain;
using BikeRental.DDD.Domain.Entities;
using BikeRental.DDD.Domain.Events;
using Carter;
using Carter.ModelBinding;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace BikeRental.DDD.Application.Commands
{
    public class CreateUser
    {        
        public record CreateUserCommand(User User) : IRequest<IResult>;
        
        public class CreateUserHandler : IRequestHandler<CreateUserCommand, IResult>
        {            
            private readonly IValidator<User> _validator;
            private readonly IUnitOfWork _unitOfWork;
            
            public CreateUserHandler(IUnitOfWork unitOfWork, IValidator<User> validator)
            {
                _unitOfWork = unitOfWork;
                _validator = validator;
            }
            public async Task<IResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
            {
                var result = _validator.Validate(request.User);
                if (!result.IsValid)
                {
                    return Results.ValidationProblem(result.GetValidationProblems());
                }

                _unitOfWork.UserRepository.AddUser(request.User);

                request.User.DomainEvents.Add(new UserCreateEvent(request.User,
                    $"The user {request.User.Name} {request.User.Surname} - {request.User.UserName} ({request.User.Email}) has been created succesfully."));

                if (await _unitOfWork.Complete())
                    return Results.Ok(request);

                return Results.BadRequest("Problem creating the user");
            }
        }        
    }
}
