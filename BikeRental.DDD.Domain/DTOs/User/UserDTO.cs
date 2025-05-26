using System;
using System.Collections.Generic;

namespace BikeRental.DDD.Domain.DTOs.User
{   
    public record class UserDTO
    {
        public string? Username { get; init; }
        public string? Email { get; init; }
        public string? KnownAs { get; init; }
        public string? Gender { get; init; }
        public string? Token { get; init; }
        public string? PhotoUrl { get; init; }
    }
}