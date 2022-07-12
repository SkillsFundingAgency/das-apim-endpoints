using System.Collections.Generic;

namespace SFA.DAS.Forecasting.InnerApi.Responses
{
    public class GetFrameworksListResponse
    {
        public IEnumerable<GetFrameworksListItem> Frameworks { get; set; }
    }
}