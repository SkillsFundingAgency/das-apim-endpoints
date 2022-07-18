using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApimDeveloper.Configuration;
using SFA.DAS.ApimDeveloper.InnerApi.Requests;
using SFA.DAS.ApimDeveloper.InnerApi.Responses;
using SFA.DAS.ApimDeveloper.Interfaces;

namespace SFA.DAS.ApimDeveloper.Application.ApiSubscriptions.Queries.GetApiProductSubscriptions
{
    public class GetApiProductSubscriptionsQueryHandler : IRequestHandler<GetApiProductSubscriptionsQuery, GetApiProductSubscriptionsQueryResult>
    {
        private readonly IApimDeveloperApiClient<ApimDeveloperApiConfiguration> _apimDeveloperApiClient;
        private readonly IApimApiService _apimApiService;

        public GetApiProductSubscriptionsQueryHandler (IApimDeveloperApiClient<ApimDeveloperApiConfiguration> apimDeveloperApiClient, IApimApiService apimApiService)
        {
            _apimDeveloperApiClient = apimDeveloperApiClient;
            _apimApiService = apimApiService;
        }
        
        public async Task<GetApiProductSubscriptionsQueryResult> Handle(GetApiProductSubscriptionsQuery request, CancellationToken cancellationToken)
        {
            var productsTask =
                _apimApiService.GetAvailableProducts(request.AccountType);

            var subscriptionsTask =
                _apimDeveloperApiClient.Get<GetApiProductSubscriptionsResponse>(
                    new GetApiProductSubscriptionsRequest(request.AccountIdentifier));

            await Task.WhenAll(productsTask, subscriptionsTask);
            
            return new GetApiProductSubscriptionsQueryResult
            {
                Products = productsTask.Result.Products,
                Subscriptions = subscriptionsTask.Result.Subscriptions
            };
        }
    }
}