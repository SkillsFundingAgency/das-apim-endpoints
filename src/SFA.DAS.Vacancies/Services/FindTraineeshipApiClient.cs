using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Vacancies.Configuration;
using SFA.DAS.Vacancies.Interfaces;

namespace SFA.DAS.Vacancies.Services
{
    public class FindTraineeshipApiClient : IFindTraineeshipApiClient<FindTraineeshipApiConfiguration>
    {
        private readonly IInternalApiClient<FindTraineeshipApiConfiguration> _apiClient;

        public FindTraineeshipApiClient(IInternalApiClient<FindTraineeshipApiConfiguration> apiClient)
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
            throw new System.NotImplementedException();
        }

        public Task<PagedResponse<TResponse>> GetPaged<TResponse>(IGetPagedApiRequest request)
        {
            throw new System.NotImplementedException();
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

        public Task<ApiResponse<TResponse>> PostWithResponseCode<TResponse>(IPostApiRequest request)
        {
            throw new System.NotImplementedException();
        }

        public Task<ApiResponse<string>> PatchWithResponseCode<TData>(IPatchApiRequest<TData> request)
        {
            throw new System.NotImplementedException();
        }
    }
}
