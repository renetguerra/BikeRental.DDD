using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikeRental.DDD.Domain.DTOs.User;

namespace BikeRental.DDD.Domain.DTOs.Rental
{
    public record class CustomerRentalHistoryDTO
    {
        public UserDTO Customer { get; set; }
        public List<RentalHistoryDTO> Rentals { get; set; } = new();
    }
}
