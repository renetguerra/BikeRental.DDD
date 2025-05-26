using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeRental.DDD.Domain.Entities
{
    public class Rental: IHasDomainEvent
    {
        public Guid Id { get; set; }
        public int BikeId { get; set; }
        public int CustomerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsReturned => EndDate.HasValue;

        public virtual Bike Bike { get; set; }
        public virtual User Customer { get; set; }

        public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
    }
}
