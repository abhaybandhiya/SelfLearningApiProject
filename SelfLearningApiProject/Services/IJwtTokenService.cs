namespace SelfLearningApiProject.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(string username);
    }
}