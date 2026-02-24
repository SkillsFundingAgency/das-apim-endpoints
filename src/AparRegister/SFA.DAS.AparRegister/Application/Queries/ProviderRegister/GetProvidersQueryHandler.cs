using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Roatp;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp.Common;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AparRegister.Application.Queries.ProviderRegister;

public class GetProvidersQueryHandler : IRequestHandler<GetProvidersQuery, GetProvidersQueryResult>
{
    private readonly IRoatpServiceApiClient<RoatpConfiguration> _apiClient;

    public GetProvidersQueryHandler(IRoatpServiceApiClient<RoatpConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }
    public async Task<GetProvidersQueryResult> Handle(GetProvidersQuery request, CancellationToken cancellationToken)
    {
        var response = await _apiClient.GetWithResponseCode<GetOrganisationsResponse>(new GetOrganisationsRequest());

        response.EnsureSuccessStatusCode();

        IEnumerable<OrganisationResponse> filteredList = response.Body.Organisations.Where(o => o.Status is OrganisationStatus.Active or OrganisationStatus.ActiveNoStarts or OrganisationStatus.OnBoarding);

        return new GetProvidersQueryResult
        {
            RegisteredProviders = filteredList
        };
    }
}
