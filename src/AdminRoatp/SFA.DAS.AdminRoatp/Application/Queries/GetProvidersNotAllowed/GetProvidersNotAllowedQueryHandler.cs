using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.AdminRoatp.InnerApi.Responses;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetProvidersNotAllowed;

public class GetProvidersNotAllowedQueryHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _courseManagementApiClient, ILogger<GetProvidersNotAllowedQueryHandler> _logger) : IRequestHandler<GetProvidersNotAllowedQuery, GetAllowedProvidersResponse>
{
    public async Task<GetAllowedProvidersResponse> Handle(GetProvidersNotAllowedQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling get providers not allowed by course request for {LarsCode}", request.larsCode);

        var response = await _courseManagementApiClient.GetWithResponseCode<GetAllowedProvidersResponse>(new GetProvidersNotAllowedRequest(request.larsCode));

        response.EnsureSuccessStatusCode();

        return response.Body;
    }
}
