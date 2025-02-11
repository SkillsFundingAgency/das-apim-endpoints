﻿using System.Collections.Generic;
using System.Net;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Threading.Tasks;

namespace SFA.DAS.SharedOuterApi.Services
{
    public class EducationalOrganisationApiClient(IInternalApiClient<EducationalOrganisationApiConfiguration> client)
        : IEducationalOrganisationApiClient<EducationalOrganisationApiConfiguration>
    {
        public Task<TResponse> Get<TResponse>(IGetApiRequest request)
        {
            return client.Get<TResponse>(request);
        }

        public Task<IEnumerable<TResponse>> GetAll<TResponse>(IGetAllApiRequest request)
        {
            return client.GetAll<TResponse>(request);
        }

        public Task<HttpStatusCode> GetResponseCode(IGetApiRequest request)
        {
            return client.GetResponseCode(request);
        }

        public Task<ApiResponse<TResponse>> GetWithResponseCode<TResponse>(IGetApiRequest request)
        {
            return client.GetWithResponseCode<TResponse>(request);
        }

        public Task<TResponse> Post<TResponse>(IPostApiRequest request)
        {
            throw new System.NotImplementedException();
        }

        public Task Post<TData>(IPostApiRequest<TData> request)
        {
            throw new System.NotImplementedException();
        }

        public Task Delete(IDeleteApiRequest request)
        {
            return client.Delete(request);
        }

        public Task Patch<TData>(IPatchApiRequest<TData> request)
        {
            return client.Patch(request);
        }

        public Task Put(IPutApiRequest request)
        {
            return client.Put(request);
        }

        public Task Put<TData>(IPutApiRequest<TData> request)
        {
            return client.Put(request);
        }

        public Task<ApiResponse<TResponse>> PostWithResponseCode<TResponse>(IPostApiRequest request, bool includeResponse = true)
        {
            return client.PostWithResponseCode<TResponse>(request, includeResponse);
        }

        public Task<ApiResponse<string>> PatchWithResponseCode<TData>(IPatchApiRequest<TData> request)
        {
            return client.PatchWithResponseCode(request);
        }

        public Task<PagedResponse<TResponse>> GetPaged<TResponse>(IGetPagedApiRequest request)
        {
            return client.GetPaged<TResponse>(request);
        }
        public Task<ApiResponse<TResponse>> PutWithResponseCode<TResponse>(IPutApiRequest request)
        {
            throw new System.NotImplementedException();
        }

        public Task<ApiResponse<TResponse>> PatchWithResponseCode<TData, TResponse>(IPatchApiRequest<TData> request, bool includeResponse = true)
        {
            throw new System.NotImplementedException();
        }
    }
}