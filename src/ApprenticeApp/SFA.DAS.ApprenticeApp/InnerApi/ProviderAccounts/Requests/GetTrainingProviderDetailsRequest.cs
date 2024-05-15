using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeApp.InnerApi.ProviderAccounts.Requests
{
    public class GetTrainingProviderDetailsRequest : IGetApiRequest
    {
        private readonly long _trainingProviderId;

        public GetTrainingProviderDetailsRequest(long trainingProviderId)
        {
            _trainingProviderId = trainingProviderId;
        }

        public string GetUrl => $"api/v1/search?searchterm={_trainingProviderId}";
    }
}