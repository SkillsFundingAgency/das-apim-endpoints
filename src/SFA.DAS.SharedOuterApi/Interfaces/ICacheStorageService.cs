using System;
using System.Threading.Tasks;

namespace SFA.DAS.SharedOuterApi.Interfaces
{
    public interface ICacheStorageService
    {
        Task<T> RetrieveFromCache<T>(string key);
        Task SaveToCache<T>(string key, T item, int expirationInHours);
        Task DeleteFromCache(string key);
        Task SaveToCache<T>(string key, T item, TimeSpan expiryTimeFromNow);
    }
}
