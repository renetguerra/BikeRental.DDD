using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeRental.DDD.Domain.Helpers
{
    public class UserParams : PaginationParams
    {
        public string? Name { get; set; } = "";
        public string? Surname { get; set; } = "";
        public string? Username { get; set; } = "";
        public string? Email { get; set; } = "";
        public string? KnownAs { get; set; } = "";
        public string? PhoneNumber { get; set; } = "";
        public string? CurrentUsername { get; set; }
        public string? Gender { get; set; }      
        public string? OrderBy { get; set; } = "lastActive";
    }
}
