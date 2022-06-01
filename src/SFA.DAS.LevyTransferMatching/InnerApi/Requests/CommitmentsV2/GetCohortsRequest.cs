using Microsoft.AspNetCore.WebUtilities;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.InnerApi.Requests.CommitmentsV2
{
    public class GetCohortsRequest : IGetApiRequest
    {
        public long? AccountId { get; set; }
        public long? ProviderId { get; set; }

        public string GetUrl
        {
            get
            {
                var queryParameters = new Dictionary<string, string>();

                if (AccountId.HasValue)
                {
                    queryParameters.Add("accountId", AccountId.Value.ToString());
                }

                if (ProviderId.HasValue)
                {
                    queryParameters.Add("providerId", ProviderId.Value.ToString());
                }

                return QueryHelpers.AddQueryString("api/cohorts", queryParameters);
            }
        }
    }
}
