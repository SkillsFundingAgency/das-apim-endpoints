using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests
{
    public class GetChangeOfPartyRequestsRequest : IGetApiRequest
    {
        public readonly long ApprenticeshipId;
        public byte Status { get; set; }

        public GetChangeOfPartyRequestsRequest(long apprenticeshipId)
        {
            ApprenticeshipId = apprenticeshipId;
        }

        public string GetUrl => $"api/apprenticeships/{ApprenticeshipId}/change-of-party-requests";
    }
}
