namespace SelfLearningApiProject.Services
{
    using SelfLearningApiProject.Entities;
    using System.Threading.Tasks;

    public interface IAuthService
    {
        Task<User?> ValidateUserAsync(string username, string password);
        Task<User> CreateUserAsync(User user);
        Task<User?> GetUserByUsernameAsync(string username);
    }
}
