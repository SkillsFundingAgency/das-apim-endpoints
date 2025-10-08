using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Roatp;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Net;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetOrganisation;
public class GetOrganisationQueryHandler(IRoatpServiceApiClient<RoatpConfiguration> _apiClient, ILogger<GetOrganisationQueryHandler> _logger) : IRequestHandler<GetOrganisationQuery, GetOrganisationResponse?>
{
    public async Task<GetOrganisationResponse?> Handle(GetOrganisationQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Get Organisation request received for Ukprn {Ukprn}", request.ukprn);

        var result = await _apiClient.GetWithResponseCode<GetOrganisationResponse>(new GetOrganisationRequest(request.ukprn));

        if (result.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        result.EnsureSuccessStatusCode();

        return result.Body;
    }
}