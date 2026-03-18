using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.InnerApi.Requests.Reference
{
    public class GetLevelsRequest : IGetAllApiRequest
    {
        public string GetAllUrl => "reference/levels";
    }
}
