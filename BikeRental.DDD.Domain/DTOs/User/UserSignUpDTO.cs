using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeRental.DDD.Domain.DTOs.User
{
    public record class UserSignUpDTO
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string PhotoUrl { get; set; }
        public string KnownAs { get; set; }
        public string Gender { get; set; }
        public string Provider { get; set; }
    }    
}
