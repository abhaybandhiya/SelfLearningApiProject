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

        // Yeh method username ke basis par user ko database se fetch karta hai aur return karta hai
        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            // Yeh line database me Users table me se pehla user fetch karti hai jiska username match karta hai provided username ke sath 
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        // Yeh method naya user create karta hai database me aur changes ko save karta hai 
        public async Task CreateAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}
