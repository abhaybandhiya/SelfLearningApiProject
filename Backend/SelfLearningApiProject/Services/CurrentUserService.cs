using System.Security.Claims;

namespace SelfLearningApiProject.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public string? Username { get; }

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            Username = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
        }
    }
}
