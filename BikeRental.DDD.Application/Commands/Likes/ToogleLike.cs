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
using BikeRental.DDD.Domain.Events.Photo;
using BikeRental.DDD.Domain.Events.Likes;

namespace BikeRental.DDD.Application.Commands
{
    public class ToogleLike
    {        
        public record ToggleUserLikeCommand(int UserId, int BikeId) : IRequest<IResult>;
        
        public class CreateLikeHandler : IRequestHandler<ToggleUserLikeCommand, IResult>
        {
            private readonly IValidator<Like> _validator;
            private readonly IUnitOfWork _unitOfWork;

            public CreateLikeHandler(IUnitOfWork unitOfWork, IValidator<Like> validator)
            {
                _unitOfWork = unitOfWork;
                _validator = validator;
            }
            public async Task<IResult> Handle(ToggleUserLikeCommand request, CancellationToken cancellationToken)
            {
                var existingLike = await _unitOfWork.LikesRepository.GetUserBikeLike(request.UserId, request.BikeId);
                
                if (existingLike == null)
                {
                    var like = new Like
                    {
                        UserId = request.UserId,
                        BikeId = request.BikeId
                    };

                    like.DomainEvents.Add(new LikeEvent(like, $"You liked user {request.UserId}"));

                    _unitOfWork.LikesRepository.AddLike(like);
                }
                else
                {
                    _unitOfWork.LikesRepository.DeleteLike(existingLike);
                }

                if (await _unitOfWork.Complete())
                    return Results.Ok(new { liked = existingLike == null });

                return Results.BadRequest("Failed to update like");
            }
        }
    }
}
