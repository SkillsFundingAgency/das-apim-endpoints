using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderCoursesService;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ProviderCoursesService;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.SharedOuterApi.Services
{
    public class ProviderCoursesService : IProviderCoursesService
    {
        private readonly IInternalApiClient<ProviderCoursesApiConfiguration> _client;

        public ProviderCoursesService(IInternalApiClient<ProviderCoursesApiConfiguration> client) => _client = client;

        public async Task<List<ProviderCourse>> GetProviderCourses(long trainingProviderId)
        {
            var providerCoursesResponse = await _client.GetWithResponseCode<List<ProviderCourse>>(new GetProviderCoursesRequest(trainingProviderId));

            if (providerCoursesResponse.StatusCode != System.Net.HttpStatusCode.OK)
                throw new HttpRequestContentException(providerCoursesResponse.ErrorContent, providerCoursesResponse.StatusCode);

            return providerCoursesResponse.Body;
        }
    }
}