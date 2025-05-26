using BikeRental.DDD.Domain.DTOs.Like;
using BikeRental.DDD.Domain.Entities;
using BikeRental.DDD.Domain.Helpers;

namespace BikeRental.DDD.Domain.IRepositories
{
    public interface ILikesRepository
    {
        /// <summary>
        /// Agrega un nuevo registro de Like.
        /// </summary>
        /// <param name="like">Entidad o registro like a añadir</param>
        void AddLike(Like like);
        void DeleteLike(Like like);

        Task<Like> GetUserBikeLike(int sourceUserId, int targetBikeId);
        Task<User> GetUserWithLikes(int userId);
        Task<PagedList<LikeDTO>> GetBikeLikes(LikesParams likesParams);

        Task<IEnumerable<int>> GetCurrentUserLikeBikeIds(int currentUserId);
    }
}