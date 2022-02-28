using System.Collections.Generic;
using Microsoft.AspNetCore.WebUtilities;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
{
    public class GetApplicationsRequest : IGetApiRequest
    {
        public int? PledgeId { get; set; }
        public long? AccountId { get; set; }
        public string ApplicationStatusFilter { get; set; }

        public string GetUrl
        {
            get
            {
                var queryParameters = new Dictionary<string, string>();

                if (PledgeId.HasValue)
                {
                    queryParameters.Add("pledgeId", PledgeId.Value.ToString());
                }

                if (AccountId.HasValue)
                {
                    queryParameters.Add("accountId", AccountId.Value.ToString());
                }

                if (ApplicationStatusFilter != null)
                {
                    queryParameters.Add("applicationStatusFilter", ApplicationStatusFilter);
                }

                return QueryHelpers.AddQueryString("applications", queryParameters);
            }
        }
    }
}
