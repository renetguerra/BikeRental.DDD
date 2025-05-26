using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BikeRental.DDD.Domain.Entities
{
    public class Bike: IHasDomainEvent
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Type { get; set; }
        public int Year { get; set; }
        public bool IsAvailable { get; set; }
        public decimal Price { get; set; }
        public virtual ICollection<BikePhoto> BikePhotos { get; set; } = new List<BikePhoto>();
        public virtual ICollection<Like> LikedByUsers { get; set; } = new List<Like>();        
        public virtual ICollection<Rental> Rentals { get; set; } = new List<Rental>();
        public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
    }
}
