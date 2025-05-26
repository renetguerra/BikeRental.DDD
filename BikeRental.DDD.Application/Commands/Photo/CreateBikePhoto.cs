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
using BikeRental.DDD.Domain.IServices;
using BikeRental.DDD.Domain.DTOs.Photo;
using AutoMapper;

namespace BikeRental.DDD.Application.Commands
{
    public class CreateBikePhoto
    {        
        public record CreatePhotoCommand(int BikeId, IFormFile File) : IRequest<IResult>;
        
        public class CreatePhotoHandler : IRequestHandler<CreatePhotoCommand, IResult>
        {           
            private readonly IValidator<BikePhoto> _validator;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IPhotoService _photoService;
            private readonly IMapper _mapper;

            public CreatePhotoHandler(IUnitOfWork unitOfWork, IPhotoService photoService, IMapper mapper, IValidator<BikePhoto> validator)
            {
                _unitOfWork = unitOfWork;
                _photoService = photoService;
                _mapper = mapper;
                _validator = validator;
            }
            public async Task<IResult> Handle(CreatePhotoCommand request, CancellationToken cancellationToken)
            {
                var bike = await _unitOfWork.BikeRepository.GetBikeByIdAsync(request.BikeId);
                if (bike == null) return Results.NotFound();

                var result = await _photoService.AddPhotoAsync(request.File);
                if (result.Error != null) return Results.BadRequest(result.Error.Message);

                var photo = new BikePhoto
                {
                    Url = result.SecureUrl.AbsoluteUri,
                    PublicId = result.PublicId,
                    BikeId = bike.Id,
                    IsMain = bike.BikePhotos.Count == 0
                };

                var validation = _validator.Validate(photo);
                if (!validation.IsValid)                
                    return Results.ValidationProblem(validation.GetValidationProblems());                

                _unitOfWork.PhotoRepository.AddBikePhoto(photo);
                photo.DomainEvents.Add(new PhotoCreatedEvent(photo, $"The photo has been created succesfully."));

                if (await _unitOfWork.Complete())
                {
                    var photoDto = _mapper.Map<PhotoDTO>(photo);
                    return Results.Ok(photoDto);
                }

                return Results.BadRequest("Problem creating the photo");
            }
        }        

    }
}
