using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.SharedOuterApi.Interfaces
{
    public interface IApiClient<T>
    {
        Task<TResponse> Get<TResponse>(IGetApiRequest request);
        Task<IEnumerable<TResponse>> GetAll<TResponse>(IGetAllApiRequest request);
        Task<HttpStatusCode> GetResponseCode(IGetApiRequest request);
        Task<TResponse> Post<TResponse>(IPostApiRequest request);
        Task<TResponse> Patch<TResponse>(IPostApiRequest request);
        Task Delete(IDeleteApiRequest request);
        Task Patch(IPatchApiRequest request);
    }
}
