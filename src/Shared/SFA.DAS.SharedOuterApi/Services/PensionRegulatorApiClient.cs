﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.SharedOuterApi.Services;

[ExcludeFromCodeCoverage]
public class PensionRegulatorApiClient : IPensionRegulatorApiClient<PensionRegulatorApiConfiguration>
{
    private readonly IInternalApiClient<PensionRegulatorApiConfiguration> _apiClient;

    public PensionRegulatorApiClient(IInternalApiClient<PensionRegulatorApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public Task<TResponse> Get<TResponse>(IGetApiRequest request)
    {
        return _apiClient.Get<TResponse>(request);
    }

    public Task<HttpStatusCode> GetResponseCode(IGetApiRequest request)
    {
        return _apiClient.GetResponseCode(request);
    }

    public Task<ApiResponse<TResponse>> GetWithResponseCode<TResponse>(IGetApiRequest request)
    {
        return _apiClient.GetWithResponseCode<TResponse>(request);
    }

    public Task<IEnumerable<TResponse>> GetAll<TResponse>(IGetAllApiRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<PagedResponse<TResponse>> GetPaged<TResponse>(IGetPagedApiRequest request)
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

    public Task Delete(IDeleteApiRequest request)
    {
        throw new NotImplementedException();
    }

    public Task Patch<TData>(IPatchApiRequest<TData> request)
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

    public Task<ApiResponse<TResponse>> PostWithResponseCode<TResponse>(IPostApiRequest request, bool includeResponse = true)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<string>> PatchWithResponseCode<TData>(IPatchApiRequest<TData> request)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<TResponse>> PutWithResponseCode<TResponse>(IPutApiRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<TResponse>> PatchWithResponseCode<TData, TResponse>(IPatchApiRequest<TData> request, bool includeResponse = true)
    {
        throw new NotImplementedException();
    }
}