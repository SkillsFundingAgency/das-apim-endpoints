using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.FindApprenticeshipTraining.Application.Configuration;
using SFA.DAS.FindApprenticeshipTraining.Application.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Services
{
    public class CourseApiClient : ICoursesApiClient<CoursesApiConfiguration>
    {
        private readonly IApiClient<CoursesApiConfiguration> _apiClient;

        public CourseApiClient (IApiClient<CoursesApiConfiguration> apiClient)
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

        public Task<string> Ping()
        {
            throw new System.NotImplementedException();
        }
    }
}