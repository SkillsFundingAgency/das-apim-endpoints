using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApimDeveloper.Interfaces;

namespace SFA.DAS.ApimDeveloper.Application.ApiProducts.Queries.GetApiProduct
{
    public class GetApiProductQueryHandler : IRequestHandler<GetApiProductQuery, GetApiProductQueryResult>
    {
        private readonly IApimApiService _apimApiService;

        public GetApiProductQueryHandler (IApimApiService apimApiService)
        {
            _apimApiService = apimApiService;
        }
        
        public async Task<GetApiProductQueryResult> Handle(GetApiProductQuery request, CancellationToken cancellationToken)
        {
            var result = await _apimApiService.GetAvailableProducts("Documentation");
            var product = result.Products.FirstOrDefault(c => c.Name.Equals(request.ProductName, StringComparison.CurrentCultureIgnoreCase));

            return new GetApiProductQueryResult
            {
                Product = product
            };
        }
    }
}