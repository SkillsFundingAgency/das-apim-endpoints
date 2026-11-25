using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.AdminRoatp.InnerApi.Responses;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetProviderNotRestrictedApprenticeships;

public class GetProviderNotRestrictedApprenticeshipsQueryHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _courseManagementApiClient, ILogger<GetProviderNotRestrictedApprenticeshipsQueryHandler> _logger) : IRequestHandler<GetProviderNotRestrictedApprenticeshipsQuery, GetProviderNotRestrictedApprenticeshipsResponse>
{
    public async Task<GetProviderNotRestrictedApprenticeshipsResponse> Handle(GetProviderNotRestrictedApprenticeshipsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handle GetProviderNotRestrictedApprenticeships request for Ukprn {Ukprn}", request.Ukprn);

        var response = await _courseManagementApiClient.GetWithResponseCode<GetProviderNotRestrictedApprenticeshipsResponse>(new GetProviderNotRestrictedApprenticeshipsRequest(request.Ukprn));

        response.EnsureSuccessStatusCode();

        return response.Body;
    }
}
