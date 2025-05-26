namespace BikeRental.DDD.Domain.DTOs.Message
{
    public record class CreateMessageDTO
    {
        public string RecipientUsername { get; set; }
        public string Content { get; set; }
    }
}