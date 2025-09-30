using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Roatp;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Net;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetOrganisation;
public class GetOrganisationQueryHandler : IRequestHandler<GetOrganisationQuery, GetOrganisationQueryResponse>
{
    private readonly IRoatpServiceApiClient<RoatpConfiguration> _apiClient;
    public GetOrganisationQueryHandler(IRoatpServiceApiClient<RoatpConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }
    public async Task<GetOrganisationQueryResponse> Handle(GetOrganisationQuery request, CancellationToken cancellationToken)
    {
        var result = await _apiClient.GetWithResponseCode<GetOrganisationResponse>(new GetOrganisationRequest(request.ukprn));

        if (result.StatusCode == HttpStatusCode.BadRequest)
        {
            throw new ApiResponseException(HttpStatusCode.NotFound, result.ErrorContent);
        }

        result.EnsureSuccessStatusCode();

        //GetOrganisationQueryResponse response = result.Body;

        return result.Body;
    }
}
