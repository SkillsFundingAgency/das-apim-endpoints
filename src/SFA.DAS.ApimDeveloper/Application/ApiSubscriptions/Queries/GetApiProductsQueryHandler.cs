using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApimDeveloper.Configuration;
using SFA.DAS.ApimDeveloper.InnerApi.Requests;
using SFA.DAS.ApimDeveloper.InnerApi.Responses;
using SFA.DAS.ApimDeveloper.Interfaces;

namespace SFA.DAS.ApimDeveloper.Application.ApiSubscriptions.Queries
{
    public class GetApiProductsQueryHandler : IRequestHandler<GetApiProductsQuery, GetApiProductsQueryResult>
    {
        private readonly IApimDeveloperApiClient<ApimDeveloperApiConfiguration> _apimDeveloperApiClient;

        public GetApiProductsQueryHandler (IApimDeveloperApiClient<ApimDeveloperApiConfiguration> apimDeveloperApiClient)
        {
            _apimDeveloperApiClient = apimDeveloperApiClient;
        }
        
        public async Task<GetApiProductsQueryResult> Handle(GetApiProductsQuery request, CancellationToken cancellationToken)
        {
            var result =
                await _apimDeveloperApiClient.Get<GetAvailableApiProductsResponse>(
                    new GetAvailableApiProductsRequest(request.AccountType));

            return new GetApiProductsQueryResult
            {
                Products = result.Products
            };
        }
    }
}