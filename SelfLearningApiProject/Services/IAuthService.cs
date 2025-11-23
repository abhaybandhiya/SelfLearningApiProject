namespace SelfLearningApiProject.Services
{
    using SelfLearningApiProject.Entities;
    using System.Threading.Tasks;

    public interface IAuthService
    {
        Task<User?> ValidateUserAsync(string username, string password);
        Task<User> CreateUserAsync(User user);
        Task<User?> GetUserByUsernameAsync(string username);
        // Yeh method user ke refresh token aur expiry time ko update karta hai
        Task UpdateUserRefreshTokenAsync(User user, string refreshToken, DateTime expiryTime);
        Task<User?> GetUserByRefreshTokenAsync(string refreshToken);

    }
}
