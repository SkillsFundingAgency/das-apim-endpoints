using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.SharedOuterApi.Interfaces
{
    public interface IApiClient<T>
    {
        Task<TResponse> Get<TResponse>(IGetApiRequest request);
        Task<IEnumerable<TResponse>> GetAll<TResponse>(IGetAllApiRequest request);
    }
}
