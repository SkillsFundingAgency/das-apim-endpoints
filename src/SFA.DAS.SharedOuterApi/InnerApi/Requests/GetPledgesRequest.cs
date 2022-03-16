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

            filters.Add("page", "0");
            filters.Add("pageSize", int.MaxValue.ToString());

            this.GetUrl = $"pledges{filters.ToQueryString()}";
        }

        public GetPledgesRequest(int page, int pageSize)
        {
            GetUrl = $"pledges?page={page}&pageSize={pageSize}";
        }

        public string GetUrl { get; set; }
    }
}