using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
{
    public class GetPledgesRequest : IGetApiRequest
    {
        public GetPledgesRequest(IEnumerable<string> sectors = null, long? accountId = null)
        {
            this.GetUrl = "pledges";

            if (sectors != null && sectors.Any())
            {
                foreach (var sector in sectors)
                {
                    this.GetUrl = this.GetUrl + (sector == sectors.First() ? "?" : "&") + "sectors=" + sector;
                }
            }

            if (accountId.HasValue)
            {
                this.GetUrl = this.GetUrl + (this.GetUrl.Contains("?") ? "&" : "?") + "accountId=" + accountId;
            }

        }

        public string GetUrl { get; set; }
    }
}