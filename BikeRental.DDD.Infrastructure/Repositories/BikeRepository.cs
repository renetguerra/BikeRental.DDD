using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BikeRental.DDD.Domain.DTOs.Bike;
using BikeRental.DDD.Domain.DTOs.User;
using BikeRental.DDD.Domain.Entities;
using BikeRental.DDD.Domain.Helpers;
using BikeRental.DDD.Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace BikeRental.DDD.Infrastructure.Repositories
{
    public class BikeRepository(DataContext context, IMapper mapper) : IBikeRepository
    {
        public async Task<IEnumerable<Bike>> GetAvailableBikesAsync()
        {
            return await context.Bikes
                .Include(b => b.BikePhotos)
                .ToListAsync();
        }

        public async Task<Bike> GetBikeByIdAsync(int bikeId)
        {
            return await context.Bikes
                .Include(b => b.BikePhotos)
                .FirstOrDefaultAsync(b => b.Id == bikeId);
        }
       
        public async Task<PagedList<BikeDTO>> GetBikesAsync(BikeParams bikeParams)
        {
            var query = context.Bikes.AsQueryable();

            if (!String.IsNullOrEmpty(bikeParams.Model))
                query = query.Where(b => b.Model == bikeParams.Model);
            if (!String.IsNullOrEmpty(bikeParams.Brand))
                query = query.Where(b => b.Brand == bikeParams.Brand);
            if (!String.IsNullOrEmpty(bikeParams.Type))
                query = query.Where(b => b.Type == bikeParams.Type);
            if (bikeParams.Year > 0)
                query = query.Where(b => b.Year == bikeParams.Year);

            query = query.Where(b => b.IsAvailable == bikeParams.IsAvailable);

            query = bikeParams.OrderBy switch
            {
                "type" => query.OrderByDescending(b => b.Type),
                "model" => query.OrderByDescending(b => b.Model),
                _ => query.OrderByDescending(b => b.Brand)
            };

            query = query.IgnoreQueryFilters();

            return await PagedList<BikeDTO>.CreateAsync(
                query.AsNoTracking().ProjectTo<BikeDTO>(mapper.ConfigurationProvider),
                bikeParams.PageNumber,
                bikeParams.PageSize);
        }

        public async Task<BikeDTO?> GetBikeAsync(int id)
        {            
            var query = context.Bikes
                .Include(x => x.BikePhotos)
            .Where(x => x.Id == id)
                .ProjectTo<BikeDTO>(mapper.ConfigurationProvider)
                .AsQueryable().IgnoreQueryFilters();            

            return await query.FirstOrDefaultAsync();
        }

        public async Task<PagedList<Bike>> GetBikesByTextFilterAsync(BikeParams bikeParams)
        {
            var bikes = context.Bikes.AsQueryable();

            if (bikes != null && bikes.Count() > 0 && bikeParams != null)
                bikes = bikes.Where(b => b.Model.Contains(bikeParams.Model)
                            || b.Brand.Contains(bikeParams.Brand)
                            || b.Type.Contains(bikeParams.Type)
                            || b.Year == bikeParams.Year);

            bikes = bikes.OrderByDescending(b => b.Model);

            return await PagedList<Bike>.CreateAsync(bikes, bikeParams.PageNumber, bikeParams.PageSize);
        }

        public void Add(Bike bike)
        {
            context.Bikes.Add(bike);
        }

        public void Update(Bike bike)
        {
            context.Bikes.Attach(bike);
            context.Entry(bike).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            var bike = context.Bikes.Find(id);

            context.Bikes.Attach(bike);
            context.Entry(bike).State = EntityState.Modified;
        }

        public void Remove(Bike bike)
        {
            context.Bikes.Remove(bike);
        }
    }
}
