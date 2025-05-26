using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeRental.DDD.Domain.DTOs.Rental
{
    public record RentalHistoryCustomerDTO
    {        
        public int BikeId { get; init; }
        
        public int UserId { get; init; }
        public string? Username { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? PhotoUrl { get; set; }
        public DateTime StartDate { get; init; }
        public DateTime? EndDate { get; init; }
        public bool IsReturned { get; init; }        
    }
}
