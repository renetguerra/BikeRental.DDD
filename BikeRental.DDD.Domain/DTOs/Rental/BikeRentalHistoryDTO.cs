using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikeRental.DDD.Domain.DTOs.Bike;
using BikeRental.DDD.Domain.DTOs.User;

namespace BikeRental.DDD.Domain.DTOs.Rental
{
    public record class BikeRentalHistoryDTO
    {
        public BikeDTO Bike { get; set; }
        public List<RentalHistoryCustomerDTO> Rentals { get; set; } = new();
    }
}
