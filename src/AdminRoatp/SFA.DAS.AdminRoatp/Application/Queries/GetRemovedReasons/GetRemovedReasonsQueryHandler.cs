using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Roatp;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetRemovedReasons;
public class GetRemovedReasonsQueryHandler(IRoatpServiceApiClient<RoatpConfiguration> _apiClient) : IRequestHandler<GetRemovedReasonsQuery, GetRemovedReasonsResponse>
{
    public async Task<GetRemovedReasonsResponse> Handle(GetRemovedReasonsQuery request, CancellationToken cancellationToken)
    {
        var response = await _apiClient.GetWithResponseCode<GetRemovedReasonsResponse>(new GetRemovedReasonsRequest());

        response.EnsureSuccessStatusCode();

        return new() { ReasonsForRemoval = response.Body.ReasonsForRemoval };
    }
}