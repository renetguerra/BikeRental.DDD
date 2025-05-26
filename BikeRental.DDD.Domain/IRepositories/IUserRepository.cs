using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikeRental.DDD.Domain.DTOs.User;
using BikeRental.DDD.Domain.Entities;
using BikeRental.DDD.Domain.Helpers;

namespace BikeRental.DDD.Domain.IRepositories
{
    public interface IUserRepository
    {
        Task<bool> UserExists(string email, string username);
        
        /// <summary>
        /// Agrega un nuevo registro de User.
        /// </summary>
        /// <param name="user">Entidad o registro User a añadir</param>
        void AddUser(User user);

        /// <summary>
        /// Actualiza un registro de la base de datos.
        /// </summary>
        /// <param name="user">User a actualizar</param>
        /// <returns></returns>
        void Update(User user);

        /// <summary>
        /// Establece el borrado lógico de un registro de tipo User.
        /// </summary>
        /// <param name="id">Identificador del registro User a borrar</param>
        /// <returns>Identificador del registro User a borrar</returns>        
        void DeleteUser(int id);

        /// <summary>
        /// Establece el borrado de un registro de tipo User.
        /// </summary>
        /// <param name="user">Usuario a borrar</param>
        /// <returns></returns>        
        void RemoveUser(User user);

        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByUsernameAsync(string username);
        Task<PagedList<CustomerDTO>> GetMembersAsync(UserParams userParams);
        Task<CustomerDTO?> GetCustomerAsync(string username, bool isCurrentUser);
        Task<PagedList<User>> GetUsersByTextFilterAsync(UserParams userParams);
        Task<User> GetUserAsync(int id, bool isCurrentUser);
        Task<string> GetUserGenderByUserName(string username);
        //Task<User?> GetUserByPhotoId(int photoId);
    }
}
