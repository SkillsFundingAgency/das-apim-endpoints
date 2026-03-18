using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests
{
    public class GetAccountsRequest : IGetApiRequest
    {
        public string GetUrl => "/accounts";
    }
}
