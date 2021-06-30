using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.InnerApi.Requests.Tags
{
    public class GetSectorsRequest : IGetAllApiRequest
    {
        public string GetAllUrl => "tags/sectors";
        public string Version { get; }
    }
}
