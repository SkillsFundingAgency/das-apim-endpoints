using SFA.DAS.SharedOuterApi.Helpers;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
{
    public class GetPledgesRequest : IGetApiRequest
    {
        public GetPledgesRequest(long? accountId = null, IEnumerable<string> sectors = null)
        {
            this.GetUrl = $"pledges{QuerystringHelper.GetFormattedQuerystring("sectors",sectors, new Dictionary<string, string>(){ { "accountId", accountId.ToString() } })}";
        }

        public string GetUrl { get; set; }
    }
}