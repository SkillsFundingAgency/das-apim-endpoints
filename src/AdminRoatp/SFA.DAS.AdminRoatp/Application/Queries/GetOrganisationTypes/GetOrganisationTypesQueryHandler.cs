using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Roatp;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetOrganisationTypes;
public class GetOrganisationTypesQueryHandler(IRoatpServiceApiClient<RoatpConfiguration> _apiClient, ILogger<GetOrganisationTypesQueryHandler> _logger) : IRequestHandler<GetOrganisationTypesQuery, GetOrganisationTypesResponse>
{
    public async Task<GetOrganisationTypesResponse> Handle(GetOrganisationTypesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handle GetOrganisationTypes request");

        var result = await _apiClient.GetWithResponseCode<GetOrganisationTypesResponse>(new GetOrganisationTypesRequest());

        result.EnsureSuccessStatusCode();

        return new() { OrganisationTypes = result.Body.OrganisationTypes };
    }
}