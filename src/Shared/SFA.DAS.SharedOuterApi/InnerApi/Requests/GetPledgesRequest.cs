using System.Collections.Generic;
using System.Collections.Specialized;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
{
    public class GetPledgesRequest : IGetApiRequest
    {
        public GetPledgesRequest(long? accountId = null, IEnumerable<string> sectors = null, string pledgeStatusFilter = null, int page = 1, int? pageSize = null, string sortBy = null)
        {
            AccountId = accountId;

            var filters = sectors != null ? sectors.ToNameValueCollection("sectors") : new NameValueCollection();
            if (accountId.HasValue)
            {
                filters.Add("accountId", accountId.ToString());
            }
            if (sortBy != null && sortBy != "")
            {
                filters.Add("sortBy", sortBy);
            }
            if (pledgeStatusFilter != null && pledgeStatusFilter != "")
            {
                filters.Add("pledgeStatusFilter", pledgeStatusFilter);
            }
            filters.Add("page", page.ToString());
            if (pageSize != null)
            {
                filters.Add("pageSize", pageSize.ToString());
            }
            GetUrl = $"pledges{filters.ToQueryString()}";
        }

        public string GetUrl { get; set; }
        public long? AccountId { get; }
    }
}