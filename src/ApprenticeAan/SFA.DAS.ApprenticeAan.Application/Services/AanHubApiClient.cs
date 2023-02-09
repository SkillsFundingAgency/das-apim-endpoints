using SFA.DAS.ApprenticeAan.Api.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace SFA.DAS.ApprenticeAan.Api.Services;

[ExcludeFromCodeCoverage]
public class AanHubApiClient : IAanHubApiClient<AanHubApiConfiguration>
{
    public Task Delete(IDeleteApiRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<TResponse> Get<TResponse>(IGetApiRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TResponse>> GetAll<TResponse>(IGetAllApiRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<PagedResponse<TResponse>> GetPaged<TResponse>(IGetPagedApiRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<HttpStatusCode> GetResponseCode(IGetApiRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<TResponse>> GetWithResponseCode<TResponse>(IGetApiRequest request)
    {
        throw new NotImplementedException();
    }

    public Task Patch<TData>(IPatchApiRequest<TData> request)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<string>> PatchWithResponseCode<TData>(IPatchApiRequest<TData> request)
    {
        throw new NotImplementedException();
    }

    public Task<TResponse> Post<TResponse>(IPostApiRequest request)
    {
        throw new NotImplementedException();
    }

    public Task Post<TData>(IPostApiRequest<TData> request)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<TResponse>> PostWithResponseCode<TResponse>(IPostApiRequest request, bool includeResponse = true)
    {
        throw new NotImplementedException();
    }

    public Task Put(IPutApiRequest request)
    {
        throw new NotImplementedException();
    }

    public Task Put<TData>(IPutApiRequest<TData> request)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<TResponse>> PutWithResponseCode<TResponse>(IPutApiRequest request)
    {
        throw new NotImplementedException();
    }
}
