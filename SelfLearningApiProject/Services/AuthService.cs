﻿using SelfLearningApiProject.Entities;
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

        // Yeh method user ko validate karta hai username aur password ke basis par aur agar valid hai to user object return karta hai
        public async Task<User?> ValidateUserAsync(string username, string password)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);

            if (user == null) return null;

            // ye password ko verify karta hai jo hashed password ke against hai database me stored hai BCrypt ka use karke agar password match karta hai to user return karo, warna null return karo
            bool verified = BCrypt.Net.BCrypt.Verify(password, user.Password);
            return verified ? user : null;
        }

        // Yeh method naya user create karta hai database me aur password ko hash karta hai
        public async Task<User> CreateUserAsync(User user)
        {
            // Password ko hash karo BCrypt ka use karke taaki secure rahe
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            await _userRepository.CreateAsync(user);
            return user;
        }

        // Yeh method username ke basis par user ko fetch karta hai
        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _userRepository.GetUserByUsernameAsync(username);
        }
    }
}
