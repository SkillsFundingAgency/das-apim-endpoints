using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;
using SFA.DAS.AdminAan.Configuration;
using SFA.DAS.AdminAan.Infrastructure;

namespace SFA.DAS.AdminAan.Api.Services;

public class ReferenceDataApiClient : IReferenceDataApiClient<ReferenceDataApiConfiguration>
{
    private readonly IInternalApiClient<ReferenceDataApiConfiguration> _client;

    public ReferenceDataApiClient(IInternalApiClient<ReferenceDataApiConfiguration> client)
    {
        _client = client;
    }
    public Task<TResponse> Get<TResponse>(IGetApiRequest request)
    {
        return _client.Get<TResponse>(request);
    }

    public Task<IEnumerable<TResponse>> GetAll<TResponse>(IGetAllApiRequest request)
    {
        throw new System.NotImplementedException();
    }

    public Task<HttpStatusCode> GetResponseCode(IGetApiRequest request)
    {
        return _client.GetResponseCode(request);
    }

    public Task<ApiResponse<TResponse>> GetWithResponseCode<TResponse>(IGetApiRequest request)
    {
        return _client.GetWithResponseCode<TResponse>(request);
    }

    public Task<TResponse> Post<TResponse>(IPostApiRequest request)
    {
        throw new System.NotImplementedException();
    }

    public Task Delete(IDeleteApiRequest request)
    {
        throw new System.NotImplementedException();
    }

    public Task Patch<TData>(IPatchApiRequest<TData> request)
    {
        throw new System.NotImplementedException();
    }

    public Task Put(IPutApiRequest request)
    {
        throw new System.NotImplementedException();
    }

    public Task Put<TData>(IPutApiRequest<TData> request)
    {
        throw new System.NotImplementedException();
    }

    public Task<ApiResponse<TResponse>> PostWithResponseCode<TResponse>(IPostApiRequest request, bool includeResponse = true)
    {
        throw new System.NotImplementedException();
    }

    public Task<ApiResponse<string>> PatchWithResponseCode<TData>(IPatchApiRequest<TData> request)
    {
        throw new System.NotImplementedException();
    }

    public Task<PagedResponse<TResponse>> GetPaged<TResponse>(IGetPagedApiRequest request)
    {
        throw new System.NotImplementedException();
    }
    public Task Post<TData>(IPostApiRequest<TData> request)

    {
        throw new System.NotImplementedException();
    }
    public Task<ApiResponse<TResponse>> PutWithResponseCode<TResponse>(IPutApiRequest request)
    {
        throw new System.NotImplementedException();
    }

    public Task GetSchools(string searchTerm, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}