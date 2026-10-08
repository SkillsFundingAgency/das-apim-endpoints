using System.Net;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.AdminRoatp.InnerApi.Models;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Roatp;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Roatp;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Roatp.Common;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetOrganisation;

public class GetOrganisationQueryHandler(IRoatpServiceApiClient<RoatpConfiguration> _apiClient, IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _courseManagementApiClient, ILogger<GetOrganisationQueryHandler> _logger) : IRequestHandler<GetOrganisationQuery, GetOrganisationQueryResult?>
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

        if (response.Body.ProviderType == ProviderType.Main)
        {
            var providerCourseTypes = await _courseManagementApiClient.GetWithResponseCode<List<ProviderCourseTypeModel>>(new GetProviderCourseTypesRequest(request.ukprn));

            providerCourseTypes.EnsureSuccessStatusCode();

            result.AllowedCourseTypes = providerCourseTypes.Body.Select(x => (AllowedCourseTypeModel)x);
        }

        if (result.Status != OrganisationStatus.Removed) return result;

        result.RemovedDate = response.Body.StatusDate;

        return result;
    }
}
