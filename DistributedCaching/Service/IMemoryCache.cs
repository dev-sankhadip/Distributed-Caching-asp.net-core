namespace DistributedCaching.Service
{
    public interface IMemoryCache
    {
        void setCache<T>(T values, string key);
        T getCache<T>(string key) where T : class;
        void removeCache(string key);
    }
}
