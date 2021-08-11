using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests
{
    public class GetApplicationsRequest : IGetAllApiRequest
    {
        public GetApplicationsRequest(int pledgeId)
        {
            PledgeId = pledgeId;
        }

        public int PledgeId { get; }

        public string GetAllUrl => $"pledges/{PledgeId}/applications";
    }
}
