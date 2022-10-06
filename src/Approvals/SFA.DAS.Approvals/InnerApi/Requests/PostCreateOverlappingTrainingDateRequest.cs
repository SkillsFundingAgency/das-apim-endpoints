using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class PostCreateOverlappingTrainingDateRequest : IPostApiRequest
    {
        public readonly long ProviderId;
        public object Data { get; set; }

        public string PostUrl => $"api/overlapping-training-date-request/{ProviderId}/create";

        public PostCreateOverlappingTrainingDateRequest(long providerId, long draftApprenticeshipId, UserInfo userInfo)
        {
            ProviderId = providerId;
            Data = new CreateOverlappingTrainingDateRequest
            {
                DraftApprenticeshipId = draftApprenticeshipId,
                UserInfo = userInfo
            };
        }
    }

    public class CreateOverlappingTrainingDateRequest : SaveDataRequest
    {
        public long DraftApprenticeshipId { get; set; }
        public long PreviousApprenticeshipId { get; set; }
    }
}
