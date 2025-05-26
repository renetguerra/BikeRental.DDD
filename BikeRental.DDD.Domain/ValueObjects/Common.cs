using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeRental.DDD.Domain.ValueObjects.Common
{
    public record AddressVO(string Street, string HouseNumber, string Zip, string City, string Country);

    public record PhotoVO(string Url, bool IsMain, string? PublicId);    

}
