using SFA.DAS.ApprenticeFeedback.InnerApi.Responses;
using System.Collections.Generic;

namespace SFA.DAS.ApprenticeFeedback.Application.Queries.GetProviderAttributes
{
    public class GetProviderAttributesResult
    {
        public List<ProviderAttribute> ProviderAttributes { get; set; }
    }
}
