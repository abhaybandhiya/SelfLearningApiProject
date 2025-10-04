using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        // User registration endpoint jo naya user banata hai database me aur password ko hash karta hai AuthService me aur role set karta hai
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] LoginRequest request) // LoginRequest DTO ka use kar rahe hain jisme username, password aur optional role hota hai
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check karo ki username already exist to nahi karta database me agar karta hai to error bhejo client ko
            var existing = await _authService.GetUserByUsernameAsync(request.Username);
            if (existing != null)
                return BadRequest(new { message = "Username already exists" });

            // Naya user create karo jo database me jayega AuthService me password hashing hoga  aur role set hoga
            var user = new SelfLearningApiProject.Entities.User
            {
                Username = request.Username,
                Password = request.Password, // AuthService will hash
                Role = string.IsNullOrWhiteSpace(request.Role) ? "User" : request.Role
            };

            await _authService.CreateUserAsync(user);

            return Ok(new { message = "User registered successfully" });
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
        public string? Username { get; set; } // username aur password dono required hain
        public string? Password { get; set; } // plain text me aayega, AuthService me hash hoga
        public string? Role { get; set; } // optional role hai, agar nahi diya to "User" set hoga
    }
}
