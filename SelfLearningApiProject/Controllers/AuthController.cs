using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SelfLearningApiProject.Repositories;
using SelfLearningApiProject.Services;

namespace JwtAuthDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IAuthService _authService;
        public AuthController(IJwtTokenService jwtTokenService, IAuthService authService)
        {
            _jwtTokenService = jwtTokenService;
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // User ko validate karo (username aur password check karo) yeh method database se user ko fetch karta hai aur password check karta hai
            var user = await _authService.ValidateUserAsync(request.Username, request.Password);

            if (user != null) // Agar user valid hai to JWT token generate karo
            {
                // JWT token generate karo jo username aur role ko include karta hai aur client ko bhejo
                var token = _jwtTokenService.GenerateToken(user.Username, user.Role);
                // Client ko token bhejo response me kyaunki yeh token aage ke requests me use hoga authentication ke liye
                return Ok(new { Token = token });
            }

            return Unauthorized("Invalid username or password");
        }

        // Sirf Admin dekh sakta hai
        [Authorize(Roles = "Admin")]
        [HttpGet("admin-data")]
        public IActionResult GetAdminData()
        {
            return Ok("Yeh sirf Admin dekh sakta hai!");
        }

        // Sirf User dekh sakta hai
        // Is endpoint ko access karne ke liye valid JWT token chahiye yeh line is endpoint ko secure karti hai sirf authenticated users hi isse access kar sakte hain
        [Authorize(Roles = "User")]   
        [HttpGet("user-data")]
        public IActionResult GetSecureData()
        {
            return Ok("Yeh sirf User dekh sakta hai!");
        }

        // Dono role dekh sakte hain
        [Authorize(Roles = "Admin,User")]
        [HttpGet("common-data")]
        public IActionResult GetCommonData()
        {
            return Ok("Yeh Admin aur User dono dekh sakte hain!");
        }
    }

    // DTO for login request
    public class LoginRequest
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
