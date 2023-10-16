using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.AparRegister.InnerApi.Requests;
using SFA.DAS.AparRegister.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AparRegister.Application.ProviderRegister.Queries
{
    public class GetProvidersQueryHandler : IRequestHandler<GetProvidersQuery, GetProvidersQueryResult>
    {
        private readonly IRoatpServiceApiClient<RoatpConfiguration> _apiClient;

        public GetProvidersQueryHandler(IRoatpServiceApiClient<RoatpConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }
        public async Task<GetProvidersQueryResult> Handle(GetProvidersQuery request, CancellationToken cancellationToken)
        {
            var response = await _apiClient.GetWithResponseCode<GetProvidersResponse>(new GetProvidersRequest());

            var registeredProviders = new List<RegisteredProvider>();
            if (ApiResponseErrorChecking.IsSuccessStatusCode(response.StatusCode))
            {
                registeredProviders = response.Body.RegisteredProviders.ToList();
            }
            return new GetProvidersQueryResult
            {
                RegisteredProviders = registeredProviders
            };
        }
    }
}