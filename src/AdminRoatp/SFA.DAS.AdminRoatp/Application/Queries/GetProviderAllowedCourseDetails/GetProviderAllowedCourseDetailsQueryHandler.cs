using MediatR;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.AdminRoatp.InnerApi.Responses;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetProviderAllowedCourseDetails;

public class GetProviderAllowedCourseDetailsQueryHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _courseManagementApiClient) : IRequestHandler<GetProviderAllowedCourseDetailsQuery, GetProviderAllowedCourseDetailsResponse?>
{
    public async Task<GetProviderAllowedCourseDetailsResponse?> Handle(GetProviderAllowedCourseDetailsQuery request, CancellationToken cancellationToken)
    {
        var response = await _courseManagementApiClient.GetWithResponseCode<GetProviderAllowedCourseDetailsResponse?>(new GetProviderAllowedCourseDetailsRequest(request.Ukprn, request.LarsCode));

        response.EnsureSuccessStatusCode();

        if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
        {
            return null;
        }

        return response.Body;
    }
}