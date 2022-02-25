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
            if (accountId.HasValue)
            {
                QuerystringHelper.KeyValues = new Dictionary<string, string>(){
                    { "accountId", accountId.ToString() }
                };
            }

            if (sectors != null && sectors.Any())
            {
                QuerystringHelper.ListKey = "sectors";
                QuerystringHelper.ListValues = sectors;
            }

            this.GetUrl = $"pledges{QuerystringHelper.GetFormattedQuerystring()}";
        }

        public string GetUrl { get; set; }
    }
}