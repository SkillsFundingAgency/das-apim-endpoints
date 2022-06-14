using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Models;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Requests;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Roatp.CourseManagement.Application.Standards.Queries.GetProviderCourse
{
    public class GetProviderCourseQueryHandler : IRequestHandler<GetProviderCourseQuery,GetProviderCourseResult>
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;
        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _courseManagementApiClient;
        private readonly ILogger<GetProviderCourseQueryHandler> _logger;

        public GetProviderCourseQueryHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> courseManagementApiClient, ICoursesApiClient<CoursesApiConfiguration> coursesApiClient,ILogger<GetProviderCourseQueryHandler> logger)
        {
            _coursesApiClient = coursesApiClient;
            _courseManagementApiClient = courseManagementApiClient;
            _logger = logger;
        }

        public async Task<GetProviderCourseResult> Handle(GetProviderCourseQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get Provider Course request received for ukprn {ukprn}, LarsCode {larsCode}", request.Ukprn, request.LarsCode);

            var standardResponse = await _coursesApiClient.GetWithResponseCode<GetStandardResponse>(new GetStandardRequest(request.LarsCode));
            if (standardResponse.StatusCode != HttpStatusCode.OK)
            {
                var errorMessage =
                    $"Response status code does not indicate success: {(int)standardResponse.StatusCode} - Standard data not found for Lars Code: {request.LarsCode}";
                _logger.LogError(errorMessage);
                throw new HttpRequestContentException(errorMessage, standardResponse.StatusCode, standardResponse.ErrorContent);
            }
            var standard = standardResponse.Body;
            var courseResponse = await _courseManagementApiClient.GetWithResponseCode<GetProviderCourseResponse>(new GetProviderCourseRequest(request.Ukprn, request.LarsCode));
            if (courseResponse.StatusCode != HttpStatusCode.OK)
            {
                var errorMessage =
                   $"Response status code does not indicate success: {(int)courseResponse.StatusCode} - Provider course details not found for ukprn: {request.Ukprn} LarsCode: {request.LarsCode}";
                _logger.LogError(errorMessage);
                throw new HttpRequestContentException(errorMessage, courseResponse.StatusCode, courseResponse.ErrorContent);
            }
            var course = courseResponse.Body;
            var providerCourseLocationsResponse = await _courseManagementApiClient.GetWithResponseCode<List<GetProviderCourseLocationsResponse>>(new GetProviderCourseLocationsRequest(request.Ukprn, request.LarsCode));
            if (providerCourseLocationsResponse.StatusCode != HttpStatusCode.OK)
            {
                var errorMessage =
                   $"Response status code does not indicate success: {(int)providerCourseLocationsResponse.StatusCode} - Provider course details not found for ukprn: {request.Ukprn} LarsCode: {request.LarsCode}";
                _logger.LogError(errorMessage);
                throw new HttpRequestContentException(errorMessage, providerCourseLocationsResponse.StatusCode, providerCourseLocationsResponse.ErrorContent);
            }
            var providerCourseLocations = providerCourseLocationsResponse.Body;

            var locations = providerCourseLocations.Select(x => (ProviderCourseLocationModel)x).ToList();
            return new GetProviderCourseResult
            {
                LarsCode = course.LarsCode,
                IfateReferenceNumber = standard.IfateReferenceNumber,
                CourseName = standard.Title,
                Level = standard.Level,
                Version = standard.Version,
                RegulatorName = standard.ApprovalBody,
                Sector = standard.Route,
                StandardInfoUrl = course.StandardInfoUrl,
                ContactUsPhoneNumber = course.ContactUsPhoneNumber,
                ContactUsEmail = course.ContactUsEmail,
                ContactUsPageUrl = course.ContactUsPageUrl,
                ProviderCourseLocations = locations,
                IsApprovedByRegulator = course.IsApprovedByRegulator
            };
        }
    }
}
