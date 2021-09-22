using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
{
    public class GetApplicationsRequest : IGetApiRequest
    {
        public GetApplicationsRequest(int pledgeId)
        {
            PledgeId = pledgeId;
            GetUrl = $"applications?pledgeId={pledgeId}";
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
