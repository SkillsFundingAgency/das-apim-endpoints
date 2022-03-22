﻿using System.Collections.Generic;
using System.Collections.Specialized;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

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

        public GetPledgesRequest()
        {
            GetUrl = $"pledges";
        }

        public string GetUrl { get; set; }
    }
}