using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApimDeveloper.Configuration;
using SFA.DAS.ApimDeveloper.InnerApi.Requests;
using SFA.DAS.ApimDeveloper.InnerApi.Responses;
using SFA.DAS.ApimDeveloper.Interfaces;

namespace SFA.DAS.ApimDeveloper.Application.ApiSubscriptions.Queries.GetApiProductSubscription
{
    public class GetApiProductSubscriptionQueryHandler : IRequestHandler<GetApiProductSubscriptionQuery, GetApiProductSubscriptionQueryResult>
    {
        private readonly IApimDeveloperApiClient<ApimDeveloperApiConfiguration> _apimDeveloperApiClient;

        public GetApiProductSubscriptionQueryHandler (IApimDeveloperApiClient<ApimDeveloperApiConfiguration> apimDeveloperApiClient)
        {
            _apimDeveloperApiClient = apimDeveloperApiClient;
        }
        public async Task<GetApiProductSubscriptionQueryResult> Handle(GetApiProductSubscriptionQuery request, CancellationToken cancellationToken)
        {
            var productsTask =
                _apimDeveloperApiClient.Get<GetAvailableApiProductsResponse>(
                    new GetAvailableApiProductsRequest(request.AccountType));

            var subscriptionsTask =
                _apimDeveloperApiClient.Get<GetApiProductSubscriptionsResponse>(
                    new GetApiProductSubscriptionsRequest(request.AccountIdentifier));

            await Task.WhenAll(productsTask, subscriptionsTask);
            
            var product = productsTask.Result.Products.FirstOrDefault(c => c.Id.Equals(request.ProductId));
            var subscription = subscriptionsTask.Result.Subscriptions.FirstOrDefault(c => c.Name.Equals(request.ProductId));

            return new GetApiProductSubscriptionQueryResult
            {
                Product = product,
                Subscription = subscription
            };
        }
    }
}