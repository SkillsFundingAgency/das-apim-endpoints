using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.RequestApprenticeTraining
{
    public class GetRegionsRequest : IGetApiRequest
    {
        public string GetUrl => $"api/regions";
    }
}
