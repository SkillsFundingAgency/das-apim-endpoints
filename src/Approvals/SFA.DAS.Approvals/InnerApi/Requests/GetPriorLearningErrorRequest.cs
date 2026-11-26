using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class GetPriorLearningErrorRequest : IGetApiRequest
    {
        public long CohortId { get; }
        public string GetUrl => $"api/cohorts/{CohortId}/prior-learning-errors";

        public GetPriorLearningErrorRequest(long cohortId)
        {
            CohortId = cohortId;
        }
    }
}