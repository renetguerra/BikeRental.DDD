using BikeRental.DDD.Domain.Entities;
using BikeRental.DDD.Domain.Helpers;
using BikeRental.DDD.Domain.IRepositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using FluentValidation;
using BikeRental.DDD.Domain.Validators;
using BikeRental.DDD.Domain.DTOs.Like;

namespace BikeRental.DDD.Infrastructure.Repositories
{
    public class LikesRepository(DataContext context, IMapper mapper) : ILikesRepository
    {       
        /// <summary>
        /// Adiciona un like a la bbdd.
        /// </summary>
        /// <param name="like">Like a añadir</param>
        public void AddLike(Like like)
        {
            context.Likes.Add(like);
        }

        public void DeleteLike(Like like)
        {
            context.Likes.Remove(like);
        }

        public async Task<Like> GetUserBikeLike(int sourceUserId, int targetBikeId)
        {
             return await context.Likes.FindAsync(sourceUserId, targetBikeId);
        }

        public async Task<User> GetUserWithLikes(int userId)
        {
            return await context.Users
                .Include(u => u.LikedBikes)
                    .ThenInclude(l => l.Bike)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<PagedList<LikeDTO>> GetBikeLikes(LikesParams likesParams)
        {
            var likes = context.Likes.AsQueryable();

            if (likesParams.Predicate == "liked")
            {
                likes = likes.Where(l => l.UserId == likesParams.UserId);
            }            

            var liked = likes
                .Include(l => l.Bike)
                    .ThenInclude(b => b.BikePhotos)
                .Select(l => new LikeDTO
                (                    
                    l.Bike.Brand,
                    l.Bike.Model,
                    l.Bike.Type,
                    l.Bike.Year,
                    l.Bike.IsAvailable,
                    l.Bike.BikePhotos.FirstOrDefault(p => p.IsMain).Url ?? "",
                    l.CreatedAt,
                    l.BikeId,
                    l.UserId
                ));

            return await PagedList<LikeDTO>.CreateAsync(liked, likesParams.PageNumber, likesParams.PageSize);
        }

        public async Task<IEnumerable<int>> GetCurrentUserLikeBikeIds(int currentUserId)
        {
            return await context.Likes
                .Where(x => x.UserId == currentUserId)
                .Select(x => x.BikeId)
                .ToListAsync();
        }
    }
}