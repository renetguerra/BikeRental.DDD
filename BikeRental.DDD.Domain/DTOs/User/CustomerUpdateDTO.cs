using BikeRental.DDD.Domain.DTOs.Common;

namespace BikeRental.DDD.Domain.DTOs.User
{
    public record class CustomerUpdateDTO
    {
        public string? UserName { get; set; }
        public string Introduction { get; set; }
        public AddressDTO Address { get; set; }
    }    
}