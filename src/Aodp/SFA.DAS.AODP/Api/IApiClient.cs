using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.AODP.Api
{
    public interface IApiClient
    {
        Task<TResponse?> Delete<TResponse>(IDeleteApiRequest request);
        Task<TResponse> Get<TResponse>(IGetApiRequest request);
        Task PostWithResponseCode(IPostApiRequest request);
        Task<TResponse?> PostWithResponseCode<TResponse>(IPostApiRequest request);
        Task<TResponse> Put<TResponse>(IPutApiRequest request);
        Task<ApiResponse<TResponse>> PutWithResponseCode<TResponse>(IPutApiRequest request);
    }
}