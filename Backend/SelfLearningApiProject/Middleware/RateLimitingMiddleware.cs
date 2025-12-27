using Microsoft.Extensions.Caching.Memory;
using System.Net;

namespace SelfLearningApiProject.Middleware
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next; // next middleware ya controller ke liye yeh delegate he jo next component ko call karta he
        private readonly IMemoryCache _cache;   // ye cache service ke liye he

        private const int LIMIT = 10;        // max requests
        private const int WINDOW = 60;       // seconds

        public RateLimitingMiddleware(RequestDelegate next, IMemoryCache cache) // constructor
        {
            _next = next;  // dependency injection se next middleware milta he
            _cache = cache; // dependency injection se cache milta he
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            var cacheKey = $"rate_limit_{ip}";

            var requestCount = _cache.Get<int?>(cacheKey) ?? 0;

            if (requestCount >= LIMIT)
            {
                context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests; // 429 status code too many requests rate limit exceeded client ne bahut requests bheji he server ne unhe reject kar diya he 
                await context.Response.WriteAsync("Too many requests. Try again later.");
                return;
            }

            _cache.Set(cacheKey, requestCount + 1, TimeSpan.FromSeconds(WINDOW)); // increment request count with expiration cache me request count set karte he aur expiration time set karte he

            await _next(context);
        }
    }
}