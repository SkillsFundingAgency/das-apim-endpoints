using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.RoatpCourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetStandardInformation;

public class GetStandardInformationQueryHandler : IRequestHandler<GetStandardInformationQuery, GetStandardInformationQueryResult>
{
    private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _roatpCourseManagementApiClient;
    private readonly ILogger<GetStandardInformationQueryHandler> _logger;

    public GetStandardInformationQueryHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> roatpCourseManagementApiClient, ILogger<GetStandardInformationQueryHandler> logger)
    {
        _roatpCourseManagementApiClient = roatpCourseManagementApiClient;
        _logger = logger;
    }

    public async Task<GetStandardInformationQueryResult> Handle(GetStandardInformationQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetStandardInformation request for {LarsCode}", request.LarsCode);

        var standardResponse = await _roatpCourseManagementApiClient.GetWithResponseCode<GetCourseDetailsResponse>(new GetCourseDetailsRequest(request.LarsCode));

        standardResponse.EnsureSuccessStatusCode();

        return standardResponse.Body;
    }
}