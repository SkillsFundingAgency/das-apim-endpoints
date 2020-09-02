using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.SharedOuterApi.Interfaces
{
    public interface IGetApiClient<T>
    {
        Task<TResponse> Get<TResponse>(IGetApiRequest request, bool ensureSuccessResponseCode = true);
    }
}
