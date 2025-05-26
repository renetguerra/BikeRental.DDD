using BikeRental.DDD.Domain.Entities;

namespace BikeRental.DDD.Domain.IServices
{
    public interface ITokenService
    {
        Task<string> CreateToken(User user);
    }
}