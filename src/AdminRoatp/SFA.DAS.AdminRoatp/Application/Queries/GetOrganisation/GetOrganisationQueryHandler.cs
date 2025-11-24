using System.Net;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Roatp;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp.Common;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetOrganisation;

public class GetOrganisationQueryHandler(IRoatpServiceApiClient<RoatpConfiguration> _apiClient, ILogger<GetOrganisationQueryHandler> _logger) : IRequestHandler<GetOrganisationQuery, GetOrganisationQueryResult?>
{
    public async Task<GetOrganisationQueryResult?> Handle(GetOrganisationQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Get Organisation request received for Ukprn {Ukprn}", request.ukprn);

        var response = await _apiClient.GetWithResponseCode<OrganisationResponse>(new GetOrganisationRequest(request.ukprn));

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        GetOrganisationQueryResult result = response.Body;
        if (result.Status != OrganisationStatus.Removed) return result;

        result.RemovedDate = response.Body.StatusDate;

        return result;
    }
}
