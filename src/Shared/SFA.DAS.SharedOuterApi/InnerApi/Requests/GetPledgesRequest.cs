using System.Collections.Generic;
using System.Collections.Specialized;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
{
    public class GetPledgesRequest : IGetApiRequest
    {
        public GetPledgesRequest(long? accountId = null, IEnumerable<string> sectors = null, string pledgeStatusFilter = null)
        {
            AccountId = accountId;

            var filters = sectors != null ? sectors.ToNameValueCollection("sectors") : new NameValueCollection();
            if (accountId.HasValue)
            {
                filters.Add("accountId", accountId.ToString());
            }
            if(pledgeStatusFilter != null && pledgeStatusFilter != "")
            {
                filters.Add("pledgeStatusFilter", pledgeStatusFilter);
            }

            this.GetUrl = $"pledges{filters.ToQueryString()}";
        }

        public string GetUrl { get; set; }
        public long? AccountId { get; }
    }
}