using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
{
    public class GetApplicationsRequest : IGetApiRequest
    {
        public GetApplicationsRequest(int pledgeId)
        {
            PledgeId = pledgeId;
            GetUrl = $"pledges/{pledgeId}/applications";
        }

        public GetApplicationsRequest(long accountId)
        {
            AccountId = accountId;
            GetUrl = $"applications?accountId={accountId}";
        }

        public int PledgeId { get; }
        public long AccountId { get; }

        public string GetUrl { get; }
    }
}
