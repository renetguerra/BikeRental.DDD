using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeRental.DDD.Domain.DTOs.Rental
{
    public record RentalHistoryDTO
    {        
        public int BikeId { get; init; }
        public string? ModelBike { get; set; }
        public string? BrandBike { get; set; }
        public decimal PriceBike { get; set; }
        public string? PhotoUrl { get; set; }
        public int UserId { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime? EndDate { get; init; }
        public bool IsReturned { get; init; }        
    }
}
