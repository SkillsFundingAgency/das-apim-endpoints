using System.Collections.Generic;
using Microsoft.AspNetCore.WebUtilities;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.LevyTransferMatching
{
    public class GetApplicationsRequest : IGetApiRequest
    {
        public int? PledgeId { get; set; }
        public long? AccountId { get; set; }
        public long? SenderAccountId { get; set; }

        public string ApplicationStatusFilter { get; set; }

        public string SortOrder { get; set; }
        public string SortDirection { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = int.MaxValue;

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

                 if (SenderAccountId.HasValue)
                {
                    queryParameters.Add("senderAccountId", SenderAccountId.Value.ToString());
                }

                if (ApplicationStatusFilter != null)
                {
                    queryParameters.Add("applicationStatusFilter", ApplicationStatusFilter);
                }

                if (!string.IsNullOrWhiteSpace(SortOrder))
                {
                    queryParameters.Add("sortOrder", SortOrder);
                }

                if (!string.IsNullOrWhiteSpace(SortDirection))
                {
                    queryParameters.Add("sortDirection", SortDirection);
                }

                queryParameters.Add("page", Page.ToString());
                queryParameters.Add("pageSize", PageSize.ToString());

                return QueryHelpers.AddQueryString("applications", queryParameters);
            }
        }
    }
}
