using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Roatp;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetOrganisationTypes;
public class GetOrganisationTypesQueryHandler(IRoatpServiceApiClient<RoatpConfiguration> _apiClient) : IRequestHandler<GetOrganisationTypesQuery, GetOrganisationTypesResponse>
{
    public async Task<GetOrganisationTypesResponse> Handle(GetOrganisationTypesQuery request, CancellationToken cancellationToken)
    {
        var result = await _apiClient.GetWithResponseCode<GetOrganisationTypesResponse>(new GetOrganisationTypesRequest());

        result.EnsureSuccessStatusCode();

        return new() { OrganisationTypes = result.Body.OrganisationTypes };
    }
}
