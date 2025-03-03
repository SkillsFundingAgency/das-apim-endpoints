using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderCoursesService
{
    public class GetProviderCoursesRequest : IGetApiRequest
    {
        private readonly long _trainingProviderId;

        public GetProviderCoursesRequest(long trainingProviderId)
        {
            _trainingProviderId = trainingProviderId;
        }

        public string GetUrl => $"api/providers/{_trainingProviderId}/courses";
    }
}