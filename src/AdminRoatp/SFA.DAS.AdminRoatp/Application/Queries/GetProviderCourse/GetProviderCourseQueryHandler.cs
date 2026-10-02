using System.Net;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.AdminRoatp.InnerApi.Responses;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetProviderCourse;

public class GetProviderCourseQueryHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _courseManagementApiClient, ILogger<GetProviderCourseQueryHandler> _logger) : IRequestHandler<GetProviderCourseQuery, GetProviderCourseResponse?>
{
    public async Task<GetProviderCourseResponse?> Handle(GetProviderCourseQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetProviderCourse request for Ukprn {Ukprn} and LarsCode {LarsCode}", request.Ukprn, request.LarsCode);

        var response = await _courseManagementApiClient.GetWithResponseCode<GetProviderCourseResponse>(new GetProviderCourseRequest(request.Ukprn, request.LarsCode));

        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        return response.Body;
    }
}
