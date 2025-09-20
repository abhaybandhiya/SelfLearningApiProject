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

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SelfLearningApiProject.Services;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class JwtService : IJwtTokenService
{
    private readonly IConfiguration _config;

    public JwtService(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateToken(string username)
    {
        // 1. JWT settings read karna
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // 2. Claims add karna (user info)
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
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
}
