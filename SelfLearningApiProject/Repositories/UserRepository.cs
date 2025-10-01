using Microsoft.EntityFrameworkCore;
using SelfLearningApiProject.Data;
using SelfLearningApiProject.Entities;

namespace SelfLearningApiProject.Repositories
{
    public class UserRepository : IUserRepository
    {
        // Yeh class AppDbContext ka use karti hai, jo ki Entity Framework Core ka DbContext hai // aur database operations ke liye use hota hai
        private readonly AppDbContext _context;

        // Constructor me AppDbContext ko inject karte hain, jisse database ke sath kaam kar sakein
        public UserRepository(AppDbContext context)
        {
            // Yeh line AppDbContext ko initialize karti hai, jisse database operations perform kiye ja sakein
            _context = context;
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}
