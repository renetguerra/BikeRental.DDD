using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikeRental.DDD.Application.Extensions;
using BikeRental.DDD.Domain;
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
using BikeRental.DDD.Domain.Events.Photo;
using BikeRental.DDD.Domain.DTOs.Common;

namespace BikeRental.DDD.Application.Commands
{
    public class UpdateUserPhoto
    {
        public record SetMainPhotoCommand(int PhotoId) : IRequest<IResult>;

        // Handler
        public class SetMainPhotoHandler : IRequestHandler<SetMainPhotoCommand, IResult>
        {
            private readonly IValidator<UserPhoto> _validator;
            private readonly DataContext _context;
            private readonly IUnitOfWork _unitOfWork;

            public SetMainPhotoHandler(DataContext context, IUnitOfWork unitOfWork, IValidator<UserPhoto> validator)
            {
                _context = context;
                _unitOfWork = unitOfWork;
                _validator = validator;
            }
            public async Task<IResult> Handle(SetMainPhotoCommand request, CancellationToken cancellationToken)
            {
                var photo = await _unitOfWork.PhotoRepository.GetUserPhotoByIdAsync(request.PhotoId);
                if (photo == null || photo.UserId <= 0) return Results.NotFound();

                if (photo.IsMain) return Results.BadRequest(new { message = "This is already the main photo." });

                var user = await _unitOfWork.UserRepository.GetUserByIdAsync(photo.UserId);
                if (user == null) return Results.NotFound();

                var currentMain = user.UserPhotos.FirstOrDefault(p => p.IsMain);
                if (currentMain != null)
                {
                    currentMain.IsMain = false;
                    currentMain.DomainEvents.Add(new PhotoUpdateEvent(currentMain, "Unset as main photo"));
                }

                photo.IsMain = true;
                photo.DomainEvents.Add(new PhotoUpdateEvent(photo, "Set as main photo"));

                if (await _unitOfWork.Complete())
                    return Results.Ok(new { message = "Main photo updated successfully" });

                return Results.BadRequest("Problem updating the main photo.");
            }            

        }
    }
}
