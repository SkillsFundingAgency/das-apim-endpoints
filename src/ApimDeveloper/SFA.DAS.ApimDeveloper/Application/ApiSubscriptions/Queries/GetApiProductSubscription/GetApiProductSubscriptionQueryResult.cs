using SFA.DAS.ApimDeveloper.InnerApi.Responses;

namespace SFA.DAS.ApimDeveloper.Application.ApiSubscriptions.Queries.GetApiProductSubscription
{
    public class GetApiProductSubscriptionQueryResult
    {
        public GetAvailableApiProductItem Product { get; set; }
        public GetApiProductSubscriptionsResponseItem Subscription { get ; set ; }   
    }
}