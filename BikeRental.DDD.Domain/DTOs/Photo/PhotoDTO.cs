using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeRental.DDD.Domain.DTOs.Photo
{
    public record PhotoDTO
    (
        int Id,
        string Url,
        bool IsMain
    );
}
