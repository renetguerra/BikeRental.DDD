using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BikeRental.DDD.Domain.Entities;
using BikeRental.DDD.Domain.IRepositories;
using BikeRental.DDD.Domain.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BikeRental.DDD.Infrastructure.Repositories
{
    public class RentalRepository : IRentalRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        private readonly IValidator<Rental> _validator;
        public RentalRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _validator = new RentalValidator();
        }

        public void RentBike(Rental rental) => _context.Rentals.Add(rental);

        public async Task<Rental> GetActiveRental(int bikeId, int customerId) =>
            await _context.Rentals
                .FirstOrDefaultAsync(r => r.BikeId == bikeId && r.CustomerId == customerId && r.EndDate == null);

        public async Task<IEnumerable<Rental>> GetRentalsByCustomer(int customerId) =>
            await _context.Rentals
                .Include(r => r.Bike).ThenInclude(p => p.BikePhotos)
                .Where(r => r.CustomerId == customerId)
                .OrderByDescending(r => r.StartDate)
                .ToListAsync();

        public async Task<IEnumerable<Rental>> GetRentalsByBike(int bikeId) =>
            await _context.Rentals
                .Include(r => r.Customer).ThenInclude(c => c.UserPhotos)
                .Where(r => r.BikeId == bikeId)                
                .ToListAsync();
        public async Task<IEnumerable<Rental>> GetRentalsAsync(int? bikeId, int? customerId)
        {
            var query = _context.Rentals
                .Include(r => r.Customer).ThenInclude(c => c.UserPhotos)
                .AsQueryable();

            if (bikeId.HasValue)
                query = query.Where(r => r.BikeId == bikeId.Value);

            //if (customerId.HasValue)
            //    query = query.Where(r => r.CustomerId == customerId.Value);

            return await query.ToListAsync();
        }

        public void Update(Rental rent)
        {
            _context.Rentals.Attach(rent);
            _context.Entry(rent).State = EntityState.Modified;
        }
    }

}
