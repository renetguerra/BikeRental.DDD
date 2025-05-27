using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BikeRental.DDD.Domain;
using BikeRental.DDD.Domain.IRepositories;
using BikeRental.DDD.Infrastructure.Repositories;

namespace BikeRental.DDD.Infrastructure
{
    /// <summary>
    /// Implementation pattern Unit of work.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        public UnitOfWork(DataContext context,
            IUserRepository userRepository, 
            IBikeRepository bikeRepository, 
            IPhotoRepository photoRepository, 
            IRentalRepository rentalRepository, 
            ILikesRepository likesRepository, 
            IConnectionRepository connectionRepository)
        {
            _context = context;
            UserRepository = userRepository;
            BikeRepository = bikeRepository;
            PhotoRepository = photoRepository;
            RentalRepository = rentalRepository;
            LikesRepository = likesRepository;
            ConnectionRepository = connectionRepository;            
        }

        // Repositories
        public IUserRepository UserRepository { get; }

        public IBikeRepository BikeRepository { get; }

        public IPhotoRepository PhotoRepository { get; }
        public IRentalRepository RentalRepository { get; }

        public ILikesRepository LikesRepository { get; }
        public IConnectionRepository ConnectionRepository { get; }


        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<bool> Complete()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            _context.ChangeTracker.DetectChanges();
            var changes = _context.ChangeTracker.HasChanges();

            return changes;
        }
    }
}
