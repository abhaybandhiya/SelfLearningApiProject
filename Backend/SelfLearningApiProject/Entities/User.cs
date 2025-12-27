using System.ComponentModel.DataAnnotations;

namespace SelfLearningApiProject.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public string Role { get; set; }

        // Refresh token properties for token renewal. yeh properties refresh token functionality ke liye hain
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
