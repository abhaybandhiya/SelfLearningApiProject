using System.ComponentModel.DataAnnotations;

namespace SelfLearningApiProject.Models.DTO
{
    public class RegisterRequestDTO
    {
        [Required]
        // Username unique hona chahiye, lekin yeh validation controller mein handle karenge
        public string Username { get; set; }
            
        // Password kam se kam 6 characters ka hona chahiye
        [Required, MinLength(6)]
        public string Password { get; set; }

        // ye optional hai, agar nahi diya to default "User" set kar denge
        public string? Role { get; set; }
    }
}
