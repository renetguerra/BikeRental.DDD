using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikeRental.DDD.Domain.Entities;

namespace BikeRental.DDD.Domain.IRepositories
{
    public interface IRentalRepository
    {
        void RentBike(Rental rental);
        void Update(Rental rent);
        Task<Rental> GetActiveRental(int bikeId, int customerId);
        Task<IEnumerable<Rental>> GetRentalsByCustomer(int customerId);
        Task<IEnumerable<Rental>> GetRentalsByBike(int bikeId);
        Task<IEnumerable<Rental>> GetRentalsAsync(int? bikeId, int? customerId);
    }

}
