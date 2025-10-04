using SelfLearningApiProject.Entities;

namespace SelfLearningApiProject.Repositories
{
    public interface IUserRepository
    {
        //Task<IEnumerable<User>> GetAllUsers(); // Yeh method saare users ko database se fetch karta hai aur unhe return karta hai
        Task<User?> GetUserByUsernameAsync(string username);
        Task CreateAsync(User user);
    }
}
