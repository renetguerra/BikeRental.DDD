using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikeRental.DDD.Domain.Helpers;

namespace BikeRental.DDD.Domain.Entities
{
    public class Like : IHasDomainEvent
    {        
        public int UserId { get; set; }
        public int BikeId { get; set; }
        public virtual User User { get; set; }
        public virtual Bike Bike { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
    }
}
