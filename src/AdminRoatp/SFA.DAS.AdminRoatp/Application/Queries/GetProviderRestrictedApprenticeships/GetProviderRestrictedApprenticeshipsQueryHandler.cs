using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.AdminRoatp.InnerApi.Responses;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetProviderRestrictedApprenticeships;

public class GetProviderRestrictedApprenticeshipsQueryHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _courseManagementApiClient, ILogger<GetProviderRestrictedApprenticeshipsQueryHandler> _logger) : IRequestHandler<GetProviderRestrictedApprenticeshipsQuery, GetProviderRestrictedApprenticeshipsResponse>
{
    public async Task<GetProviderRestrictedApprenticeshipsResponse> Handle(GetProviderRestrictedApprenticeshipsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handle GetProviderRestrictedApprenticeships request for Ukprn {Ukprn}", request.Ukprn);

        var response = await _courseManagementApiClient.GetWithResponseCode<GetProviderRestrictedApprenticeshipsResponse>(new GetProviderRestrictedApprenticeshipsRequest(request.Ukprn));

        response.EnsureSuccessStatusCode();

        return response.Body;
    }
}
