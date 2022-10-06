using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests
{
    public class GetOverlapRequestRequest : IGetApiRequest
    {
        public readonly long DraftApprenticeshipId;

        public GetOverlapRequestRequest(long draftApprenticeshipId)
        {
            DraftApprenticeshipId = draftApprenticeshipId;
        }

        public string GetUrl => $"api/overlapping-training-date-request/{DraftApprenticeshipId}/getOverlapRequest";
    }
}