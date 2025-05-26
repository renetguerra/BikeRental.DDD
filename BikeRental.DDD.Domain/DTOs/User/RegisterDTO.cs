using System;
using System.ComponentModel.DataAnnotations;
using BikeRental.DDD.Domain.DTOs.Common;

namespace BikeRental.DDD.Domain.DTOs.User
{
    public record class RegisterDTO
    {
        [Required] public string Username { get; init; }
        //[Required] public string? Email { get; init; }
        [Required] public string Name { get; init; }
        [Required] public string Surname { get; init; }        
        [Required] public string KnownAs { get; init; }
        [Required] public DateTime? DateOfBirth { get; init; }
        [Required] public string Gender { get; init; }

        //public AddressDTO? Address { get; init; }

        [Required]
        [StringLength(8, MinimumLength = 4)]
        public string Password { get; init; }
    }
}