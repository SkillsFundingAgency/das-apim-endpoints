using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class GetLearnerForProviderRequest(long providerId, long learnerId) : IGetApiRequest
    {
        public string GetUrl => $"providers/{providerId}/learners/{learnerId}";
    }
}