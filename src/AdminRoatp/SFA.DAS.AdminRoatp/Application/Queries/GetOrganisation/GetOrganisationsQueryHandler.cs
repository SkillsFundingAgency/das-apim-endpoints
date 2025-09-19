using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Roatp;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetOrganisation;

public class GetOrganisationsQueryHandler(IRoatpServiceApiClient<RoatpConfiguration> _apiClient, ILogger<GetOrganisationsQueryHandler> _logger) : IRequestHandler<GetOrganisationsQuery, GetOrganisationsQueryResponse>
{
    public async Task<GetOrganisationsQueryResponse> Handle(GetOrganisationsQuery request, CancellationToken cancellationToken)
    {
        var response = await _apiClient.GetWithResponseCode<SearchOrganisationResponse>(new SearchOrganisationRequest(request.SearchTerm));

        // check if response was ok

        response.EnsureSuccessStatusCode();

        return new() { Organisations = response.Body.SearchResults.Select(c => (Organisation)c) };
    }
}