namespace TaskManager.DTOs
{
    public class JwtTokenDto
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public string RefreshToken { get; set; }
    }
}
