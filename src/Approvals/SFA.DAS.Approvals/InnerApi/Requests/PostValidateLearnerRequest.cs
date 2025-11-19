using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class PostValidateLearnerRequest : IPostApiRequest
    {
        public readonly long ProviderId;
        public long LearnerDataId { get; set; }
        public object Data { get; set; }
        public string PostUrl => $"api/providers/{ProviderId}/learners/{LearnerDataId}/validate";

        public PostValidateLearnerRequest(long providerId, long learnerDataId, ValidateLearnerApiRequest data)
        {
            ProviderId = providerId;
            LearnerDataId = learnerDataId;
            Data = data;
        }
    }
}
