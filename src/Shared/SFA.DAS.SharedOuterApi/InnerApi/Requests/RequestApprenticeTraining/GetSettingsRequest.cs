using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining
{
    public class GetSettingsRequest : IGetApiRequest
    {
        public string GetUrl => $"api/settings";
    }
}
