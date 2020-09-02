using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.SharedOuterApi.Interfaces
{
    public interface IApiClient<T>
    {
        Task<TResponse> Get<TResponse>(IGetApiRequest request);
        Task<IEnumerable<TResponse>> GetAll<TResponse>(IGetAllApiRequest request);
        Task<HttpStatusCode> GetResponseCode(IGetApiRequest request, string namedClient = default);
        Task<TResponse> Post<TResponse>(IPostApiRequest request);
        Task Delete(IDeleteApiRequest request);
        Task Patch<TData>(IPatchApiRequest<TData> request);
        Task Put(IPutApiRequest request);
        Task Put<TData>(IPutApiRequest<TData> request);
    }
}
