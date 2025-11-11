using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.TrainingProviderService
{
    public class GetTrainingProviderDetailsRequest : IGetApiRequest
    {
        private readonly long _ukprn;

        public GetTrainingProviderDetailsRequest(int ukprn)
        {
            _ukprn = ukprn;
        }

        public string GetUrl => $"organisations/{_ukprn}";
    }
}