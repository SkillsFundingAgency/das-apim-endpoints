using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Roatp;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetRemovedReasons;
public class GetRemovedReasonsQueryHandler(IRoatpServiceApiClient<RoatpConfiguration> _apiClient, ILogger<GetRemovedReasonsQueryHandler> _logger) : IRequestHandler<GetRemovedReasonsQuery, GetRemovedReasonsResponse>
{
    public async Task<GetRemovedReasonsResponse> Handle(GetRemovedReasonsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handle get all removed reasons request");

        var response = await _apiClient.GetWithResponseCode<GetRemovedReasonsResponse>(new GetRemovedReasonsRequest());

        response.EnsureSuccessStatusCode();

        return new() { ReasonsForRemoval = response.Body.ReasonsForRemoval };
    }
}