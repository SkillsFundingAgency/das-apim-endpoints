using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeFeedback.InnerApi.Requests
{
    public class GetActiveApprenticeLearnerCountRequest : IGetApiRequest
    {
        public string GetUrl => $"api/learner/{Ukprn}";
        public long Ukprn { get; set; }
    }
}
