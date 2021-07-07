using System.Threading.Tasks;

namespace SFA.DAS.SharedOuterApi.Interfaces
{
    public interface IReliableCacheStorageService
    {
        Task<T> GetData<T>(IGetApiRequest request, string cacheKey);
    }
}