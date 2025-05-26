using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeRental.DDD.Domain.Helpers
{
    public class BikeParams : PaginationParams
    {
        public string? Model { get; set; } = "";
        public string? Brand { get; set; } = "";
        public int Year { get; set; } = 0;
        public string? Type { get; set; } = "";
        public bool IsAvailable { get; set; } = true;
        public int MinPrice { get; set; } = 1;
        public int MaxPrice { get; set; }        
        public string? OrderBy { get; set; } = "lastActive";
    }
}
