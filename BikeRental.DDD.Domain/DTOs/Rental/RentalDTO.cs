using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeRental.DDD.Domain.DTOs.Rental
{
    public record RentalDTO
    {
        public Guid Id { get; init; }
        public int BikeId { get; init; }
        public int CustomerId { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime? EndDate { get; init; }
        public bool IsReturned { get; init; }        
    }
}
