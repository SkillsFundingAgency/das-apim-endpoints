using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
{
    public class GetPledgesRequest : IGetApiRequest
    {
        public GetPledgesRequest(long? accountId = null, IEnumerable<string> sectors = null)
        {
            var filters = sectors != null ? sectors.ToNameValueCollection("sectors") : new NameValueCollection();
            if (accountId.HasValue)
            {
                filters.Add("accountId", accountId.ToString());
            }
            this.GetUrl = $"pledges{filters.ToQueryString()}";
        }

        public string GetUrl { get; set; }
    }
}