namespace SelfLearningApiProject.Models.DTO
{
    public class RefreshTokenRequestDTO
    {
        // yeh properties client se refresh token request ke liye hain // jisme username aur refresh token hota hai jo user ko naya access token dene ke liye use hota hai
        public string? Username { get; set; }
        public string? RefreshToken { get; set; }   
    }
}
