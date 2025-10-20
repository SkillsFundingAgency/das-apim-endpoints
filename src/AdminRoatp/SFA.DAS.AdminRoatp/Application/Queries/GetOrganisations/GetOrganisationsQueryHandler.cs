using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Roatp;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetOrganisations;

public class GetOrganisationsQueryHandler(IRoatpServiceApiClient<RoatpConfiguration> _apiClient, ILogger<GetOrganisationsQueryHandler> _logger) : IRequestHandler<GetOrganisationsQuery, GetOrganisationsResponse>
{
    public async Task<GetOrganisationsResponse> Handle(GetOrganisationsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handle GetOrganisations request");

        var response = await _apiClient.GetWithResponseCode<GetOrganisationsResponse>(new GetOrganisationsRequest());

        response.EnsureSuccessStatusCode();

        return new() { Organisations = response.Body.Organisations };
    }
}