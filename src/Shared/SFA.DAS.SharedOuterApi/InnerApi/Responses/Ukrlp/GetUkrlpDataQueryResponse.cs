using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Models.Ukrlp;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.Ukrlp
{
    public class GetUkrlpDataQueryResponse
    {
        public bool Success { get; set; }
        public List<ProviderAddress> Results { get; set; }
    }
}
