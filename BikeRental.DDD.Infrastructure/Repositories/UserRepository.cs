using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BikeRental.DDD.Domain.DTOs.User;
using BikeRental.DDD.Domain.Entities;
using BikeRental.DDD.Domain.Helpers;
using BikeRental.DDD.Domain.IRepositories;
using Microsoft.EntityFrameworkCore;
using static StackExchange.Redis.Role;

namespace BikeRental.DDD.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public UserRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
       
        public async Task<bool> UserExists(string email, string username)
        {
            if (await _context.Users.AnyAsync(u => u.Email == email && u.UserName == username))
                return true;

            return false;            
        }

        public async Task<User> GetUserAsync(int id, bool isCurrentUser)
        {
            var query = _context.Users.Include(p => p.UserPhotos).AsQueryable();

            if (isCurrentUser)
                query = query.IgnoreQueryFilters();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<CustomerDTO?> GetCustomerAsync(string username, bool isCurrentUser)
        {
            var query = _context.Users                
                .Include(x => x.UserPhotos)                                
            .Where(x => x.UserName == username)
                .ProjectTo<CustomerDTO>(_mapper.ConfigurationProvider)
                .AsQueryable();

            if (isCurrentUser) query = query.IgnoreQueryFilters();

            return await query.FirstOrDefaultAsync();
        }

        public async Task<PagedList<CustomerDTO>> GetMembersAsync(UserParams userParams)
        {
            var query = _context.Users.AsQueryable();

            query = query.Where(u => u.UserName != userParams.CurrentUsername);
            query = query.Where(u => u.Gender == userParams.Gender);                                   

            query = userParams.OrderBy switch
            {
                "created" => query.OrderByDescending(u => u.Created),
                _ => query.OrderByDescending(u => u.LastActive)
            };

            return await PagedList<CustomerDTO>.CreateAsync(
                query.AsNoTracking().ProjectTo<CustomerDTO>(_mapper.ConfigurationProvider),
                userParams.PageNumber,
                userParams.PageSize);
        }

        public async Task<PagedList<User>> GetUsersByTextFilterAsync(UserParams userParams)
        {
            var users = _context.Users.AsQueryable();

            if (users != null && users.Count() > 0 && userParams != null)
                users = users.Where(u => u.UserName.Contains(userParams.Username)
                            || u.Email.Contains(userParams.Email)
                            || u.KnownAs.Contains(userParams.KnownAs)
                            || u.PhoneNumber.Contains(userParams.PhoneNumber));

            users = users.OrderByDescending(r => r.UserName);

            return await PagedList<User>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(u => u.UserPhotos)
                .SingleOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<string> GetUserGenderByUserName(string username)
        {
            return await _context.Users
                .Where(x => x.UserName == username)
                .Select(x => x.Gender).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users
                .Include(p => p.UserPhotos)
                .ToListAsync();
        }

        /// <summary>
        /// Adiciona un usuario a la bbdd.
        /// </summary>
        /// <param name="user">Usuario a añadir</param>
        public void AddUser(User user)
        {
            _context.Users.Add(user);
        }

        /// <summary>
        /// Actualiza un usuario de la bbdd.
        /// </summary>
        /// <param name="user">Usuario a actualizar</param>
        public void Update(User user)
        {
            _context.Users.Attach(user);
            _context.Entry(user).State = EntityState.Modified;
        }

        /// <summary>
        /// Actualiza un usuario de la bbdd. (Borrado lógico)
        /// </summary>
        /// <param name="id">Identificador del usuario a actualizar</param>
        public void DeleteUser(int id)
        {
            var user = _context.Users.Find(id);

            _context.Users.Attach(user);
            _context.Entry(user).State = EntityState.Modified;
        }

        /// <summary>
        /// Establece el borrado de un registro de tipo User
        /// </summary>
        /// <param name="user">Usuario a borrar</param>
        public void RemoveUser(User user)
        {
            _context.Users.Remove(user);
        }
    }
}
