using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.AdminRoatp.InnerApi.Responses;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetAllowedProviders;

public class GetAllowedProvidersQueryHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _courseManagementApiClient, ILogger<GetAllowedProvidersQueryHandler> _logger) : IRequestHandler<GetAllowedProvidersQuery, GetAllowedProvidersResponse>
{
    public async Task<GetAllowedProvidersResponse> Handle(GetAllowedProvidersQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling get allowed providers by course request");

        var response = await _courseManagementApiClient.GetWithResponseCode<GetAllowedProvidersResponse>(new GetAllowedProvidersRequest(request.larsCode));

        response.EnsureSuccessStatusCode();

        return response.Body;
    }
}
