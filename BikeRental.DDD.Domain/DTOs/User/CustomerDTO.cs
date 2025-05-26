using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikeRental.DDD.Domain.DTOs.Common;
using BikeRental.DDD.Domain.DTOs.Photo;
using BikeRental.DDD.Domain.ValueObjects.Common;

namespace BikeRental.DDD.Domain.DTOs.User
{
    public record class CustomerDTO
    {       
        public int Id { get; set; }
        public string? Username { get; set; }
        public int Age { get; set; }
        public string? PhotoUrl { get; set; }
        public string? KnownAs { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string? Gender { get; set; }
        public string? Introduction { get; set; }
        public string Email { get; set; }    
        public AddressDTO Address { get; set; }
        public List<PhotoDTO>? UserPhotos { get; set; }

        public CustomerDTO() { }
    }
}
