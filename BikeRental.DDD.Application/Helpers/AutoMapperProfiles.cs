using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BikeRental.DDD.Domain.DTOs.Bike;
using BikeRental.DDD.Domain.DTOs.Common;
using BikeRental.DDD.Domain.DTOs.Like;
using BikeRental.DDD.Domain.DTOs.Message;
using BikeRental.DDD.Domain.DTOs.Photo;
using BikeRental.DDD.Domain.DTOs.Rental;
using BikeRental.DDD.Domain.DTOs.User;
using BikeRental.DDD.Domain.Entities;
using BikeRental.DDD.Domain.Helpers;
using BikeRental.DDD.Domain.ValueObjects.Common;
using static BikeRental.DDD.Application.Commands.UpdateUser;


namespace BikeRental.DDD.Application.Helpers
{
    /// <summary>
    /// Mapeo de entidades usando Automapper
    /// </summary>
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AddressVO, AddressDTO>();

            CreateMap<Bike, BikeDTO>()
                .ForMember(dest => dest.PhotoUrl,
                    opt => opt.MapFrom(src => src.BikePhotos.FirstOrDefault(x => x.IsMain).Url));
            CreateMap<BikeDTO, Bike>();

            CreateMap<User, CustomerDTO>()
                .ForMember(dest => dest.PhotoUrl,
                    opt => opt.MapFrom(src => src.UserPhotos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address));

            CreateMap<PhotoVO, PhotoDTO>();
            CreateMap<UserPhoto, PhotoDTO>();
            CreateMap<PhotoDTO, UserPhoto>();

            CreateMap<BikePhoto, PhotoDTO>();
            CreateMap<PhotoDTO, BikePhoto>();

            CreateMap<CustomerUpdateDTO, User>();
            CreateMap<RegisterDTO, User>();
                //.ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Message, MessageDTO>()
                .ForMember(a => a.SenderPhotoUrl, opt => opt
                    .MapFrom(u => u.Sender.UserPhotos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(a => a.RecipientPhotoUrl, opt => opt
                    .MapFrom(u => u.Recipient.UserPhotos.FirstOrDefault(p => p.IsMain).Url));            

            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();

            CreateMap<User, UpdateUserCommand>();
            CreateMap<UpdateUserCommand, User>();

            CreateMap<CustomerUpdateDTO, User>();
            CreateMap<AddressDTO, AddressVO>()
                .ConstructUsing(dto => new AddressVO(dto.Street, dto.HouseNumber, dto.Zip, dto.City, dto.Country));


            CreateMap<DateTime, DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
            CreateMap<DateTime?, DateTime?>().ConvertUsing(d => d.HasValue ?
                DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : null);

            CreateMap<Like, LikeDTO>();
            CreateMap<LikeDTO, Like>();

            CreateMap<Rental, RentalDTO>();
            CreateMap<RentalDTO, Rental>();

        }
    }
}
