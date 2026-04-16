using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.Infrastructure.Ukrlp
{
    public class GetUkrlpDataQueryResponse
    {
        public bool Success { get; set; }
        public List<ProviderAddress> Results { get; set; }
    }
}
