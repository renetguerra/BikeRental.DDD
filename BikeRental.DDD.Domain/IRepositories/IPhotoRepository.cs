using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using BikeRental.DDD.Domain.Entities;

namespace BikeRental.DDD.Domain.IRepositories
{
    /// <summary>
    /// Interface para el repositorio de la tabla de Photo.
    /// </summary>
    public interface IPhotoRepository
    {                

        /// <summary>
        /// Obtiene un listado de photos
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<UserPhoto>> GetUserPhotosAsync(int userId);
        Task<IEnumerable<BikePhoto>> GetBikePhotosAsync(int bikeId);

        /// <summary>
        /// Obtiene un photo por su Id
        /// </summary>
        /// <param name="id">Identificador del photo</param>
        /// <returns></returns>
        Task<UserPhoto> GetUserPhotoByIdAsync(int id);
        Task<BikePhoto> GetBikePhotoByIdAsync(int id);


        /// <summary>
        /// Agrega un nuevo registro de Photo.
        /// </summary>
        /// <param name="photo">Entidad o registro photo a añadir</param>
        void AddUserPhoto(UserPhoto photo);
        void AddBikePhoto(BikePhoto photo);

        /// <summary>
        /// Actualiza un registro de la base de datos.
        /// </summary>
        /// <param name="photo">Photo a actualizar</param>
        /// <returns></returns>
        void UpdateUserPhoto(UserPhoto photo);
        void UpdateBikePhoto(BikePhoto photo);

        /// <summary>
        /// Establece el borrado lógico de un registro de tipo Photo.
        /// </summary>
        /// <param name="id">Identificador del registro photo a borrar</param>
        /// <returns>Identificador del registro photo a borrar</returns>        
        void DeleteUserPhoto(int id);
        void DeleteBikePhoto(int id);

        /// <summary>
        /// Establece el borrado de un registro de tipo Photo.
        /// </summary>
        /// <param name="photo">Photo a borrar</param>
        /// <returns></returns>        
        void RemoveUserPhoto(UserPhoto photo);
        void RemoveBikePhoto(BikePhoto photo);

    }
}
