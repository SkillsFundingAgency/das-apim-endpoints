using System.Net;
using SFA.DAS.SharedOuterApi.Models;
using System.Threading.Tasks;

namespace SFA.DAS.SharedOuterApi.Interfaces
{
    public interface IApprenticeCommitmentsApiClient<T> : IGetApiClient<T>
    {
        Task<HttpStatusCode> Patch<TRequest>(IPatchApiRequest<TRequest> request);
        Task<ApiResponse<TResponse>> PatchWithResponseCode<TRequest, TResponse>(
            IPatchApiRequest<TRequest> request);        
        Task<ApiResponse<TResponse>> PostWithResponseCode<TResponse>(IPostApiRequest request, bool includeResponse = false);
    }
}