using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BikeRental.DDD.Domain.DTOs;
using BikeRental.DDD.Domain.Entities;
using BikeRental.DDD.Domain.Helpers;
using BikeRental.DDD.Domain.IRepositories;
using BikeRental.DDD.Domain.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BikeRental.DDD.Infrastructure.Repositories
{   
    public class ConnectionRepository : IConnectionRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        
        private readonly IValidator<Connection> _validator;

        /// <summary>
        /// Inicializa una nueva instancia de la clase ConnectionRepository.
        /// </summary>
        /// <param name="context">Contexto</param>
        /// <param name="mapper">Interface del Automapper para el mapeo de entidades</param>
        public ConnectionRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;            
            _validator = new ConnectionValidator();
        }

        /// <summary>
        /// Adiciona una connection a la bbdd.
        /// </summary>
        /// <param name="connection">Connection a añadir</param>
        public async Task AddConnection(Connection connection)
        {
            _context.Connections.Add(connection);
        }

        /// <summary>
        /// Actualiza una connection de la bbdd.
        /// </summary>
        /// <param name="connection">Connection a actualizar</param>
        public void Update(Connection connection)
        {
            _context.Connections.Attach(connection);
            _context.Entry(connection).State = EntityState.Modified;
        }

        /// <summary>
        /// Actualiza una Connection de la bbdd. (Borrado lógico)
        /// </summary>
        /// <param name="id">Identificador de la Connection a actualizar</param>
        public void DeleteConnection(int id)
        {
            var connection = _context.Connections.Find(id);

            _context.Connections.Attach(connection);
            _context.Entry(connection).State = EntityState.Modified;
        }

        /// <summary>
        /// Obtiene el Connection por Id.
        /// </summary>
        /// <param name="id">Identificador del Connection</param>
        /// <returns></returns>
        public async Task<Connection> GetConnectionByIdAsync(int id)
        {
            return await _context.Connections.FindAsync(id);
        }

        /// <summary>
        /// Obtiene el listado de Connections.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Connection>> GetConnectionsAsync(string userName)
        {
            return await _context.Connections.Where(c => c.Username == userName).ToListAsync();
        }

        /// <summary>
        /// Establece el borrado de un registro de tipo Connection
        /// </summary>
        /// <param name="connection">Connection a borrar</param>
        public void RemoveConnection(Connection connection)
        {
            _context.Connections.Remove(connection);
        }        
    }
}
