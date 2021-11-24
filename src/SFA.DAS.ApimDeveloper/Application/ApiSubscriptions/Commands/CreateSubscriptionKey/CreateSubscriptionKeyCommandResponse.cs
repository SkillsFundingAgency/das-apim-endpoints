using SFA.DAS.ApimDeveloper.InnerApi.Responses;

namespace SFA.DAS.ApimDeveloper.Application.ApiSubscriptions.Commands.CreateSubscriptionKey
{
    public class CreateSubscriptionKeyCommandResponse
    {
        public GetAvailableApiProductItem Product { get; set; }
        public GetApiProductSubscriptionsResponseItem Subscription { get ; set ; }
    }
}