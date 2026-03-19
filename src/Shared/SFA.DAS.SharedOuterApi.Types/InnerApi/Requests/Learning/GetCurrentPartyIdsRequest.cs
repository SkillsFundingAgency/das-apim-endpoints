using SFA.DAS.Apim.Shared.Interfaces;

using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Learning
{
    public class GetCurrentPartyIdsRequest : IGetApiRequest
    {
        public Guid LearningKey { get; set; }
        public string GetUrl => $"{LearningKey}/currentPartyIds";
    }
}
