using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.AdminRoatp.InnerApi.Responses;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetProvidersAllowedToDeliverCourse;

public class GetProvidersAllowedToDeliverCourseQueryHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _courseManagementApiClient, ILogger<GetProvidersAllowedToDeliverCourseQueryHandler> _logger) : IRequestHandler<GetProvidersAllowedToDeliverCourseQuery, RestrictedCourseDetailsModel>
{
    public async Task<RestrictedCourseDetailsModel> Handle(GetProvidersAllowedToDeliverCourseQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling get allowed providers by course request for {LarsCode}", request.larsCode);

        var response = await _courseManagementApiClient.GetWithResponseCode<RestrictedCourseDetailsModel>(new GetAllowedProvidersRequest(request.larsCode));

        response.EnsureSuccessStatusCode();

        return response.Body;
    }
}
