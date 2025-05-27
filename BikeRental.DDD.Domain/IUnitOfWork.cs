using BikeRental.DDD.Domain.IRepositories;

namespace BikeRental.DDD.Domain;

/// <summary>
/// Interface para implementar el patrón Unit of work.
/// </summary>
public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    IBikeRepository BikeRepository { get; }        
    IPhotoRepository PhotoRepository { get; }        
    IRentalRepository RentalRepository { get; }

    ILikesRepository LikesRepository { get; }
    IConnectionRepository ConnectionRepository { get; }    


    void Add<T>(T entity) where T : class;
    void Delete<T>(T entity) where T : class;
    Task<bool> Complete();
    bool HasChanges();
}
