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

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetStandardInformation
{
    public class GetStandardInformationQueryHandler : IRequestHandler<GetStandardInformationQuery, GetStandardInformationQueryResult>
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;
        private readonly ILogger<GetStandardInformationQueryHandler> _logger;

        public GetStandardInformationQueryHandler(ICoursesApiClient<CoursesApiConfiguration> coursesApiClient, ILogger<GetStandardInformationQueryHandler> logger)
        {
            _coursesApiClient = coursesApiClient;
            _logger = logger;
        }

        public async Task<GetStandardInformationQueryResult> Handle(GetStandardInformationQuery request, CancellationToken cancellationToken)
        {
            var standardResponse = await _coursesApiClient.GetWithResponseCode<GetStandardResponse>(new GetStandardRequest(request.LarsCode));
            if (standardResponse.StatusCode != HttpStatusCode.OK)
            {
                _logger.LogError("Courses API  did not respond with success status: {statusCode} when fetching information for standard: {larscode}", standardResponse.StatusCode, request.LarsCode);
                throw new InvalidOperationException($"Courses API  did not respond with success status: {standardResponse.StatusCode} when fetching information for standard: {request.LarsCode}");
            }
            return standardResponse.Body;
        }
    }
}
