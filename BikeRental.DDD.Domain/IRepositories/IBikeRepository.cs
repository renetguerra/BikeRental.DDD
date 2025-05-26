using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikeRental.DDD.Domain.DTOs.Bike;
using BikeRental.DDD.Domain.DTOs.User;
using BikeRental.DDD.Domain.Entities;
using BikeRental.DDD.Domain.Helpers;
using CloudinaryDotNet.Actions;

namespace BikeRental.DDD.Domain.IRepositories
{
    public interface IBikeRepository
    {
        Task<IEnumerable<Bike>> GetAvailableBikesAsync();
        Task<Bike> GetBikeByIdAsync(int bikeId);
        Task<PagedList<BikeDTO>> GetBikesAsync(BikeParams bikeParams);
        Task<BikeDTO?> GetBikeAsync(int bikeId);
        Task<PagedList<Bike>> GetBikesByTextFilterAsync(BikeParams bikeParams);

        void Add(Bike bike);
        void Update(Bike bike);
        void Delete(int id);
        void Remove(Bike bike);
    }
}
