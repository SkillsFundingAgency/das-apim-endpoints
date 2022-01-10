using MediatR;

namespace SFA.DAS.ApimDeveloper.Application.ApiSubscriptions.Queries.GetApiProductSubscriptions
{
    public class GetApiProductSubscriptionsQuery : IRequest<GetApiProductSubscriptionsQueryResult>
    {
        public string AccountType { get ; set ; }
        public string AccountIdentifier { get ; set ; }
    }
}