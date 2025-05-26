namespace BikeRental.DDD.Domain.DTOs.User
{
    public record class LoginDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }    
}