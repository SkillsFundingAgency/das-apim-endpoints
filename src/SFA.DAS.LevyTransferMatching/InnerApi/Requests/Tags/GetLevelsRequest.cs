using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.InnerApi.Requests.Tags
{
    public class GetLevelsRequest : IGetAllApiRequest
    {
        public string GetAllUrl => "tags/levels";
        public string Version { get; }
    }
}
