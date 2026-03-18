using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Learning
{
    public class GetLearningStartDateRequest : IGetApiRequest
    {
        public Guid LearningKey { get; set; }
        public string GetUrl => $"{LearningKey}/startDate";
    }
}
