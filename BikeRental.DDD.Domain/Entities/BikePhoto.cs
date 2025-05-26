using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using BikeRental.DDD.Domain.DTOs.Common;
using BikeRental.DDD.Domain.ValueObjects.Common;

namespace BikeRental.DDD.Domain.Entities
{
    [Table("BikePhotos")]
    public class BikePhoto : Photo, IHasDomainEvent
    {
        public int Id { get; set; }
        public int? BikeId { get; set; }                
        [JsonIgnore]
        public virtual Bike? Bike { get; set; }

        public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
    }    
}
