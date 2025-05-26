namespace BikeRental.DDD.Domain.Helpers
{
    public class LikesParams : PaginationParams
    {
        public int UserId { get; set; }
        public string? Predicate { get; set; }
    }
}