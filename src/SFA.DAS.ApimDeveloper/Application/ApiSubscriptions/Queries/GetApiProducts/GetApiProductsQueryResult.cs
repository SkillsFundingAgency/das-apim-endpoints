using System.Collections.Generic;
using SFA.DAS.ApimDeveloper.InnerApi.Responses;

namespace SFA.DAS.ApimDeveloper.Application.ApiSubscriptions.Queries.GetApiProducts
{
    public class GetApiProductsQueryResult
    {
        public IEnumerable<GetAvailableApiProductItem> Products { get; set; }
        public IEnumerable<GetApiProductSubscriptionsResponseItem> Subscriptions { get ; set ; }
    }
}