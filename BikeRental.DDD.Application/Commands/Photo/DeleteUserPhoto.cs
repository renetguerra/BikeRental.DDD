using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikeRental.DDD.Domain;
using BikeRental.DDD.Domain.Events;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using BikeRental.DDD.Domain.Events.Photo;
using BikeRental.DDD.Domain.DTOs.Common;
using BikeRental.DDD.Domain.Entities;
using BikeRental.DDD.Domain.IServices;

namespace BikeRental.DDD.Application.Commands
{
    public class DeleteUserPhoto
    {        
        public record DeletePhotoCommand(string Username, int PhotoId) : IRequest<IResult>;

        // Handler
        public class DeletePhotoHandler : IRequestHandler<DeletePhotoCommand, IResult>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IPhotoService _photoService;
            private readonly IMediator _mediator;

            public DeletePhotoHandler(IUnitOfWork unitOfWork, IPhotoService photoService, IMediator mediator)
            {
                _unitOfWork = unitOfWork;
                _photoService = photoService;
                _mediator = mediator;
            }
            public async Task<IResult> Handle(DeletePhotoCommand request, CancellationToken cancellationToken)
            {
                var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(request.Username);
                if (user is null)
                    return Results.BadRequest("User not found");

                var photo = await _unitOfWork.PhotoRepository.GetUserPhotoByIdAsync(request.PhotoId);

                if (photo is null || photo.IsMain)
                    return Results.BadRequest("This photo cannot be deleted");

                if (photo.PublicId is not null)
                {
                    var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                    if (result.Error is not null)
                        return Results.BadRequest(result.Error.Message);
                }

                var userPhoto = user.UserPhotos.FirstOrDefault(x => x.Id == request.PhotoId);
                if (userPhoto is null)
                    return Results.BadRequest("This photo is not in the user's photos");

                user.UserPhotos.Remove(userPhoto);
                _unitOfWork.PhotoRepository.RemoveUserPhoto(photo);

                photo.DomainEvents.Add(new PhotoDeleteEvent(photo, "The photo has been deleted successfully"));

                if (await _unitOfWork.Complete())
                    return Results.Ok(new { message = "Photo deleted", photoId = request.PhotoId });

                return Results.BadRequest("Problem removing the photo");
            }
        }
    }
}
