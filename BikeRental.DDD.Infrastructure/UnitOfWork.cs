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
        private readonly IMapper _mapper;
        public UnitOfWork(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Repositories
        public IUserRepository UserRepository => new UserRepository(_context, _mapper);

        public IBikeRepository BikeRepository => new BikeRepository(_context, _mapper);

        public IPhotoRepository PhotoRepository => new PhotoRepository(_context, _mapper);
        public IRentalRepository RentalRepository => new RentalRepository(_context, _mapper);

        public ILikesRepository LikesRepository => new LikesRepository(_context, _mapper);
        public IConnectionRepository ConnectionRepository => new ConnectionRepository(_context, _mapper);
        public IMessageRepository MessageRepository => new MessageRepository(_context, _mapper);


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
