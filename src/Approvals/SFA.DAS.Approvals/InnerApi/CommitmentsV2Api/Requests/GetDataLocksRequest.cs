using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests
{
    public class GetDataLocksRequest : IGetApiRequest
    {
        public readonly long ApprenticeshipId;
        public byte Status { get; set; }

        public GetDataLocksRequest(long apprenticeshipId)
        {
            ApprenticeshipId = apprenticeshipId;
        }

        public string GetUrl => $"api/apprenticeships/{ApprenticeshipId}/datalocks";
    }
}
