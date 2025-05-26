using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikeRental.DDD.Domain;
using BikeRental.DDD.Domain.Entities;
using BikeRental.DDD.Domain.Events;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace BikeRental.DDD.Application.Commands
{
    public class DeleteUser
    {        
        public record DeleteUserCommand(string Name) : IRequest<IResult>;
        
        public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, IResult>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMediator _mediator;

            public DeleteUserHandler(IUnitOfWork unitOfWork, IMediator mediator)
            {
                _unitOfWork = unitOfWork;
                _mediator = mediator;
            }
            public async Task<IResult> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
            {
                var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(request.Name);

                if (user is null)
                {
                    return Results.NotFound();
                }
                
                user.DomainEvents.Add(new UserDeleteEvent(user, $"The user {user.UserName} has been deleted"));

                _unitOfWork.UserRepository.RemoveUser(user);

                if (await _unitOfWork.Complete())
                    return Results.Ok(user);

                return Results.BadRequest("Problem removing the user");
            }
        }
    }
}
