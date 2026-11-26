using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class GetProvidersRequest : IGetApiRequest
    {
        public string GetUrl => "api/providers/";
    }
}