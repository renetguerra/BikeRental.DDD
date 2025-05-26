using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikeRental.DDD.Domain.DTOs.Like;
using BikeRental.DDD.Domain.DTOs.Photo;
using BikeRental.DDD.Domain.DTOs.Rental;
using BikeRental.DDD.Domain.Entities;

namespace BikeRental.DDD.Domain.DTOs.Bike
{
    public record class BikeDTO
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Type { get; set; }
        public int Year { get; set; }
        public bool IsAvailable { get; set; }
        public decimal Price { get; set; }
        public string PhotoUrl { get; set; }

        public ICollection<PhotoDTO> BikePhotos { get; set; }
        //public List<LikeDTO> LikedByUsers { get; set; }
        //public List<RentalDTO> Rentals { get; set; }

        public BikeDTO() { }
    }
}
