namespace SelfLearningApiProject.Services
{
    using SelfLearningApiProject.Entities;
    using System.Threading.Tasks;

    public interface IAuthService
    {
        Task<User?> ValidateUserAsync(string username, string password);
    }
}
