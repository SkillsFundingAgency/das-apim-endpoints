using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.RoatpCourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Net;
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
        var standardResponse = await _roatpCourseManagementApiClient.GetWithResponseCode<GetStandardForLarsCodeResponse>(new GetStandardForLarsCodeRequest(request.LarsCode));
        if (standardResponse.StatusCode != HttpStatusCode.OK)
        {
            _logger.LogError("Course Management API  did not respond with success status: {statusCode} when fetching information for standard: {larscode}", standardResponse.StatusCode, request.LarsCode);
            throw new InvalidOperationException($"Course Management API  did not respond with success status: {standardResponse.StatusCode} when fetching information for standard: {request.LarsCode}");
        }

        return standardResponse.Body;
    }
}