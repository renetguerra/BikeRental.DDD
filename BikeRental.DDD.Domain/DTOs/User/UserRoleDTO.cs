using System;
using System.Collections.Generic;

namespace BikeRental.DDD.Domain.DTOs.User
{
    public record class UserRoleDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string PhotoUrl { get; set; }
        public string KnownAs { get; set; }
        public string Gender { get; set; }
        public List<string> Roles { get; set; }
    }    
}