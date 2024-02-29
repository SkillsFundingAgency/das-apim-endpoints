using SFA.DAS.Approvals.Enums;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests
{
    public class GetApprenticeshipUpdatesRequest : IGetApiRequest
    {
        public readonly long ApprenticeshipId;
        public ApprenticeshipUpdateStatus Status { get; set; }

        public GetApprenticeshipUpdatesRequest(long apprenticeshipId, ApprenticeshipUpdateStatus status)
        {
            ApprenticeshipId = apprenticeshipId;
            Status = status;
        }

        public string GetUrl => $"api/apprenticeships/{ApprenticeshipId}/updates?Status={Status}";
    }
}
