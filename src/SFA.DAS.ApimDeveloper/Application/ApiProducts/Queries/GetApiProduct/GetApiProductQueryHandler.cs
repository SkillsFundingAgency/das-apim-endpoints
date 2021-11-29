using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApimDeveloper.Configuration;
using SFA.DAS.ApimDeveloper.InnerApi.Requests;
using SFA.DAS.ApimDeveloper.InnerApi.Responses;
using SFA.DAS.ApimDeveloper.Interfaces;

namespace SFA.DAS.ApimDeveloper.Application.ApiProducts.Queries.GetApiProduct
{
    public class GetApiProductQueryHandler : IRequestHandler<GetApiProductQuery, GetApiProductQueryResult>
    {
        private readonly IApimDeveloperApiClient<ApimDeveloperApiConfiguration> _apimDeveloperApiClient;

        public GetApiProductQueryHandler (IApimDeveloperApiClient<ApimDeveloperApiConfiguration> apimDeveloperApiClient)
        {
            _apimDeveloperApiClient = apimDeveloperApiClient;
        }
        
        public async Task<GetApiProductQueryResult> Handle(GetApiProductQuery request, CancellationToken cancellationToken)
        {
            var result =
                await _apimDeveloperApiClient.Get<GetAvailableApiProductsResponse>(
                    new GetAvailableApiProductsRequest("Documentation"));
            var product = result.Products.FirstOrDefault(c => c.Name.Equals(request.ProductName, StringComparison.CurrentCultureIgnoreCase));

            return new GetApiProductQueryResult
            {
                Product = product
            };
        }
    }
}