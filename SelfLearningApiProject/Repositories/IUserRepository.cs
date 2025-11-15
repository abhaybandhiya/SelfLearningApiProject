using SelfLearningApiProject.Entities;

namespace SelfLearningApiProject.Repositories
{
    public interface IUserRepository
    {
        //Task<IEnumerable<User>> GetAllUsers(); // Yeh method saare users ko database se fetch karta hai aur unhe return karta hai
        Task<User?> GetUserByUsernameAsync(string username); // Yeh method username ke basis par ek user ko database se fetch karta hai

        // Yeh method naya user create karta hai database me
        Task CreateAsync(User user);
    }
}
