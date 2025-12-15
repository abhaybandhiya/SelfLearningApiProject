//namespace SelfLearningApiProject.Services
//{
//    public class JwtService : IJwtTokenService
//    {
//        private readonly IConfiguration _config;

//        public JwtService(IConfiguration config)
//        {
//            _config = config;
//        }

//        public string GenerateToken(string username)
//        {
//            // yaha token generation ka code aayega
//            return "dummy-token"; // abhi ke liye
//        }
//    }
//}

using Microsoft.IdentityModel.Tokens;
using SelfLearningApiProject.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

public class JwtService : IJwtTokenService
{
    private readonly IConfiguration _config;

    public JwtService(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateToken(string username, string role)
    {
        // 1. JWT settings read karna
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // 2. Claims add karna (user info)
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username), // ye subject claim hai jisme username store hota hai
            new Claim(ClaimTypes.Role, role), // ye role claim hai jisme user ka role store hota hai claim matlab ? // ek piece of information jo token me store hota hai
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // ye unique identifier claim hai har token ke liye
            new Claim(ClaimTypes.Name, username),
            // Custom claim for demo policies:
            new Claim("department", role == "Admin" ? "Management" : "Sales")
            // (abhi simple mapping rakhi hai; baad me User entity me Department aa gaya to yahin se pick kar lena)
        };

        // 3. Token banana
        var token = new JwtSecurityToken(
            issuer: _config["JwtSettings:Issuer"],
            audience: _config["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_config["JwtSettings:DurationMinutes"])),
            signingCredentials: creds
        );

        // 4. Token string return
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
