using System.Collections.Generic;
using SFA.DAS.ApimDeveloper.InnerApi.Responses;

namespace SFA.DAS.ApimDeveloper.Application.ApiSubscriptions.Queries
{
    public class GetApiProductsQueryResult
    {
        public IEnumerable<GetAvailableApiProductItem> Products { get; set; }
    }
}