using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikeRental.DDD.Domain.Helpers;
using BikeRental.DDD.Domain.ValueObjects;
using BikeRental.DDD.Domain.ValueObjects.Common;
using Microsoft.AspNetCore.Identity;

namespace BikeRental.DDD.Domain.Entities
{
    public class User : IdentityUser<int>
    {
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string KnownAs { get; set; }
        public required string Gender { get; set; }                         
        public string? Introduction { get; set; }                
        public DateTime DateOfBirth { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastActive { get; set; } = DateTime.UtcNow;

        public AddressVO? Address { get; set; }

        public virtual List<UserPhoto> UserPhotos { get; set; } = new();

        public virtual List<Like> LikedBikes { get; set; } = new();        

        public virtual List<UserRole> UserRoles { get; set; }

        public virtual ICollection<Rental> Rentals { get; set; } = new List<Rental>();

        public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
        
    }
}
