using SFA.DAS.ApimDeveloper.InnerApi.Responses;

namespace SFA.DAS.ApimDeveloper.Application.ApiSubscriptions.Queries.GetApiProduct
{
    public class GetApiProductQueryResult
    {
        public GetAvailableApiProductItem Product { get; set; }
        public GetApiProductSubscriptionsResponseItem Subscription { get ; set ; }   
    }
}