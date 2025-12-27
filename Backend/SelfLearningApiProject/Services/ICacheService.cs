namespace SelfLearningApiProject.Services
{
    public interface ICacheService
    {
        T GetData<T>(string key); // Generic method to get data from cache
        void SetData<T>(string key, T value, int durationInSeconds); // Generic method to set data in cache with expiration
        void RemoveData(string key); // Method to remove data from cache by key
    }
}
