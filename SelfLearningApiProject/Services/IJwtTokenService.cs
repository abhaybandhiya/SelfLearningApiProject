namespace SelfLearningApiProject.Services
{
    public interface IJwtTokenService
    {
        // Yeh method JWT token generate karega given username and role ke liye
        string GenerateToken(string username, string role);
        //object GenerateToken(string username);
            
        // Yeh method refresh token generate karega
        string GenerateRefreshToken();
    }
}