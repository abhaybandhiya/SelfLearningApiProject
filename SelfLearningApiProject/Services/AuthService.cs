using SelfLearningApiProject.Entities;
using SelfLearningApiProject.Repositories;
using System.Threading.Tasks;

namespace SelfLearningApiProject.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> ValidateUserAsync(string username, string password)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);

            if (user != null && user.Password == password) // yaha hashed password compare karna better hota hai
                return user;

            return null;
        }
    }
}
