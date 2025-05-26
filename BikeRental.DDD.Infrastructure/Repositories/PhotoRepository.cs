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
    public class PhotoRepository : IPhotoRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        
        private readonly IValidator<UserPhoto> _validator;

        /// <summary>
        /// Inicializa una nueva instancia de la clase PhotoRepository.
        /// </summary>
        /// <param name="context">Contexto</param>
        /// <param name="mapper">Interface del Automapper para el mapeo de entidades</param>
        public PhotoRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;            
            _validator = new UserPhotoValidator();
        }

        /// <summary>
        /// Adiciona una photo a la bbdd.
        /// </summary>
        /// <param name="photo">Photo a añadir</param>
        public void AddUserPhoto(UserPhoto photo)
        {
            _context.UserPhotos.Add(photo);
        }

        public void AddBikePhoto(BikePhoto photo)
        {
            _context.BikePhotos.Add(photo);
        }

        /// <summary>
        /// Actualiza una photo de la bbdd.
        /// </summary>
        /// <param name="photo">Photo a actualizar</param>
        public void UpdateUserPhoto(UserPhoto photo)
        {
            _context.UserPhotos.Attach(photo);
            _context.Entry(photo).State = EntityState.Modified;
        }

        public void UpdateBikePhoto(BikePhoto photo)
        {
            _context.BikePhotos.Attach(photo);
            _context.Entry(photo).State = EntityState.Modified;
        }

        /// <summary>
        /// Actualiza un photo de la bbdd. (Borrado lógico)
        /// </summary>
        /// <param name="id">Identificador del photo a actualizar</param>
        public void DeleteUserPhoto(int id)
        {
            var photo = _context.UserPhotos.Find(id);

            _context.UserPhotos.Attach(photo);
            _context.Entry(photo).State = EntityState.Modified;
        }

        public void DeleteBikePhoto(int id)
        {
            var photo = _context.BikePhotos.Find(id);

            _context.BikePhotos.Attach(photo);
            _context.Entry(photo).State = EntityState.Modified;
        }

        /// <summary>
        /// Obtiene el photo por Id.
        /// </summary>
        /// <param name="id">Identificador del photo</param>
        /// <returns></returns>
        public async Task<UserPhoto> GetUserPhotoByIdAsync(int id)
        {
            return await _context.UserPhotos.FindAsync(id);
        }
        public async Task<BikePhoto> GetBikePhotoByIdAsync(int id)
        {
            return await _context.BikePhotos.FindAsync(id);
        }

        /// <summary>
        /// Obtiene el listado de photos.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<UserPhoto>> GetUserPhotosAsync(int userId)
        {
            return await _context.UserPhotos.Where(p => p.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<BikePhoto>> GetBikePhotosAsync(int bikeId)
        {
            return await _context.BikePhotos.Where(b => b.BikeId == bikeId).ToListAsync();
        }

        /// <summary>
        /// Establece el borrado de un registro de tipo Photo
        /// </summary>
        /// <param name="photo">Photo a borrar</param>
        public void RemoveUserPhoto(UserPhoto photo)
        {
            _context.UserPhotos.Remove(photo);
        }
        public void RemoveBikePhoto(BikePhoto photo)
        {
            _context.BikePhotos.Remove(photo);
        }
    }
}
