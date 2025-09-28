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

        public AuthController(IJwtTokenService jwtTokenService)
        {
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Dummy validation (aage jaake DB se check karenge)
            if (request.Username == "admin" && request.Password == "password")
            {
                //var token = _jwtTokenService.GenerateToken(request.Username);
                var token = _jwtTokenService.GenerateToken(request.Username, "Admin");
                return Ok(new { Token = token });
            }
            else if (request.Username == "user" && request.Password == "user123")
            {
                var token = _jwtTokenService.GenerateToken(request.Username, "User");
                return Ok(new { Token = token });
            }

            return Unauthorized("Invalid username or password");
        }

        // 👇 Sirf Admin dekh sakta hai
        [Authorize(Roles = "Admin")]
        [HttpGet("admin-data")]
        public IActionResult GetAdminData()
        {
            return Ok("Yeh sirf Admin dekh sakta hai!");
        }

        //Sirf User dekh sakta hai
        // Is endpoint ko access karne ke liye valid JWT token chahiye yeh line is endpoint ko secure karti hai sirf authenticated users hi isse access kar sakte hain
        [Authorize(Roles = "User")]   
        [HttpGet("user-data")]
        public IActionResult GetSecureData()
        {
            return Ok("Yeh sirf User dekh sakta hai!");
        }

        // 👇 Dono role dekh sakte hain
        [Authorize]
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
