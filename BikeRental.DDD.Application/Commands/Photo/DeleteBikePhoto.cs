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
    public class DeleteBikePhoto
    {        
        public record DeletePhotoCommand(int BikeId, int PhotoId) : IRequest<IResult>;

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
                var bike = await _unitOfWork.BikeRepository.GetBikeByIdAsync(request.BikeId);
                if (bike is null)
                    return Results.BadRequest("Bike not found");

                var photo = await _unitOfWork.PhotoRepository.GetBikePhotoByIdAsync(request.PhotoId);

                if (photo is null || photo.IsMain)
                    return Results.BadRequest("This photo cannot be deleted");

                if (photo.PublicId is not null)
                {
                    var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                    if (result.Error is not null)
                        return Results.BadRequest(result.Error.Message);
                }

                var bikePhoto = bike.BikePhotos.FirstOrDefault(x => x.Id == request.PhotoId);
                if (bikePhoto is null)
                    return Results.BadRequest("This photo is not in the bike's photos");

                bike.BikePhotos.Remove(bikePhoto);
                _unitOfWork.PhotoRepository.RemoveBikePhoto(photo);

                photo.DomainEvents.Add(new PhotoDeleteEvent(photo, "The photo has been deleted successfully"));

                if (await _unitOfWork.Complete())
                    return Results.Ok(new { message = "Photo deleted", photoId = request.PhotoId });

                return Results.BadRequest("Problem removing the photo");
            }
        }
    }
}
