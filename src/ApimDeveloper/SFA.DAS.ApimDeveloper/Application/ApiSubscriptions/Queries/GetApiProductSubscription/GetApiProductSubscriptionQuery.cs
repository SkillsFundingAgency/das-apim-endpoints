using MediatR;

namespace SFA.DAS.ApimDeveloper.Application.ApiSubscriptions.Queries.GetApiProductSubscription
{
    public class GetApiProductSubscriptionQuery : IRequest<GetApiProductSubscriptionQueryResult>
    {
        public string ProductId { get ; set ; }
        public string AccountType { get ; set ; }
        public string AccountIdentifier { get ; set ; }
    }
}