using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Aodp.Wrapper;

public interface IMultipartFormDataSenderWrapper
{
    Task<ApiResponse<TResponse>> PostWithMultipartFormData<TData, TResponse>(IPostApiRequest<TData> request, bool includeResponse = true);
}
