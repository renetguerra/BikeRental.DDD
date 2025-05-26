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
    /// Interface para el repositorio de la tabla de Connection.
    /// </summary>
    public interface IConnectionRepository
    {

        /// <summary>
        /// Obtiene un listado de Connections
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Connection>> GetConnectionsAsync(string userName);

        /// <summary>
        /// Obtiene un Connection por su Id
        /// </summary>
        /// <param name="id">Identificador del Connection</param>
        /// <returns></returns>
        Task<Connection> GetConnectionByIdAsync(int id);


        /// <summary>
        /// Agrega un nuevo registro de Connection.
        /// </summary>
        /// <param name="connection">Entidad o registro Connection a añadir</param>
        Task AddConnection(Connection connection);

        /// <summary>
        /// Actualiza un registro de la base de datos.
        /// </summary>
        /// <param name="connection">Connection a actualizar</param>
        /// <returns></returns>
        void Update(Connection connection);

        /// <summary>
        /// Establece el borrado lógico de un registro de tipo Connection.
        /// </summary>
        /// <param name="id">Identificador del registro Connection a borrar</param>
        /// <returns>Identificador del registro Connection a borrar</returns>        
        void DeleteConnection(int id);

        /// <summary>
        /// Establece el borrado de un registro de tipo Connection.
        /// </summary>
        /// <param name="connection">Connection a borrar</param>
        /// <returns></returns>        
        void RemoveConnection(Connection connection);

    }
}
