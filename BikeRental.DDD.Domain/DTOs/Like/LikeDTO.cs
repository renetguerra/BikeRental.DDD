namespace BikeRental.DDD.Domain.DTOs.Like
{
    public record LikeDTO
    (        
        string Brand,
        string Model,
        string Type,
        int Year,
        bool Available,
        string PhotoUrl,
        DateTime CreatedAt,
        int BikeId,
        int UserId
    );     
}