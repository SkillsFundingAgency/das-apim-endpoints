using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Services
{
    public class CourseDeliveryApiClient : ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>
    {
        private readonly IApiClient<CourseDeliveryApiConfiguration> _apiClient;

        public CourseDeliveryApiClient (IApiClient<CourseDeliveryApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }
        public Task<TResponse> Get<TResponse>(IGetApiRequest request)
        {
            return _apiClient.Get<TResponse>(request);
        }

        public Task<IEnumerable<TResponse>> GetAll<TResponse>(IGetAllApiRequest request)
        {
            return _apiClient.GetAll<TResponse>(request);
        }

        public Task<HttpStatusCode> GetResponseCode(IGetApiRequest request)
        {
            return _apiClient.GetResponseCode(request);
        }
        
        public Task<TResponse> Post<TResponse>(IPostApiRequest request)
        {
            return _apiClient.Post<TResponse>(request);
        }

        public Task Delete(IDeleteApiRequest request)
        {
            return _apiClient.Delete(request);
        }

        public Task Patch(IPatchApiRequest request)
        {
            return _apiClient.Patch(request);
        }

        public Task Put(IPutApiRequest request)
        {
            return _apiClient.Put(request);
        }
    }
}