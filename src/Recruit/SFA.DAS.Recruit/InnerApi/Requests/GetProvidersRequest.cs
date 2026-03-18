using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Requests
{
    public class GetProvidersRequest : IGetApiRequest
    {
        public string GetUrl => "api/providers";
    }
}