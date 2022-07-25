using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests
{
    public class GetOverlappingApprenticeshipDetailsRequest : IGetApiRequest
    {
        public readonly long ApprenticeshipId;
        public readonly long ProviderId;
        public readonly long DraftApprenticeshipId;

        public GetOverlappingApprenticeshipDetailsRequest(long providerId, long draftApprenticeshipId)
        {
            ProviderId = providerId;
            DraftApprenticeshipId = draftApprenticeshipId;
        }

        public string GetUrl => $"api/overlapping-training-date-request/{ProviderId}/apprenticeship/details?draftApprenticeshipId={DraftApprenticeshipId}";
    }
}
