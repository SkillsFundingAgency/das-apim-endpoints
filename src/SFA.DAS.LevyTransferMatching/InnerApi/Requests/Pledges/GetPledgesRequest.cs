using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.InnerApi.Requests.Pledges
{
    public class GetPledgesRequest : IGetApiRequest
    {
        public GetPledgesRequest(long? accountId = null)
        {
            this.GetUrl = "pledges";

            if (accountId.HasValue)
            {
                this.GetUrl += $"?accountId={accountId}";
            }
        }

        public string GetUrl { get; set; }
    }
}
