using Microsoft.Extensions.Caching.Memory;

namespace SelfLearningApiProject.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;

        public CacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public T GetData<T>(string key)
        {
            _cache.TryGetValue(key, out T value);
            return value;
        }

        public void SetData<T>(string key, T value, int durationInSeconds)
        {
            var expiry = DateTimeOffset.Now.AddSeconds(durationInSeconds);
            _cache.Set(key, value, expiry);
        }

        public void RemoveData(string key)
        {
            _cache.Remove(key);
        }
    }
}
