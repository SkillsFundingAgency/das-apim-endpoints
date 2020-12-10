using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.SharedOuterApi.Interfaces
{
    public interface IApiClient<T> : IGetApiClient<T>
    {
        Task<IEnumerable<TResponse>> GetAll<TResponse>(IGetAllApiRequest request);
        Task<TResponse> Post<TResponse>(IPostApiRequest request);
        Task Delete(IDeleteApiRequest request);
        Task Patch<TData>(IPatchApiRequest<TData> request);
        Task Put(IPutApiRequest request);
        Task Put<TData>(IPutApiRequest<TData> request);
    }
}
