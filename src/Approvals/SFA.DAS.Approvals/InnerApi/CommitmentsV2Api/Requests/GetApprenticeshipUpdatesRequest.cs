using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests
{
    public class GetApprenticeshipUpdatesRequest : IGetApiRequest
    {
        public readonly long ApprenticeshipId;
        public byte Status { get; set; }

        public GetApprenticeshipUpdatesRequest(long apprenticeshipId, byte status)
        {
            ApprenticeshipId = apprenticeshipId;
            Status = status;
        }

        public string GetUrl => $"api/apprenticeships/{ApprenticeshipId}/updates?Status={Status}";
    }
}
