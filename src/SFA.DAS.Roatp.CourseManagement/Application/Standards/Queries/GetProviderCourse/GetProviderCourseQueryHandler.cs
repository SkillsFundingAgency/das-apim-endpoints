using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Models;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Requests;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
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

        public async Task<GetProviderCourseResult> Handle(GetProviderCourse.GetProviderCourseQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get Provider Course request received for ukprn {ukprn}, LarsCode {larsCode}", request.Ukprn, request.LarsCode);

            var standard = await _coursesApiClient.Get<GetStandardResponse>(new GetStandardRequest(request.LarsCode));
            if (standard == null)
            {
                _logger.LogError($"Standard data not found for Lars Code: {request.LarsCode}");
                return null;
            }
       

             var course = await _courseManagementApiClient.Get<GetProviderCourseResponse>(new GetProviderCourseRequest(request.Ukprn, request.LarsCode));
            if (course == null)
            {
                _logger.LogError($"Provider course details not found for ukprn: {request.Ukprn} LarsCode: {request.LarsCode}" );
                return null;
            }

            var providerCourseLocations = await _courseManagementApiClient.Get<List<GetProviderCourseLocationsResponse>>(new GetProviderCourseLocationsRequest(request.ProviderCourseId));
            if (!providerCourseLocations.Any())
            {
                _logger.LogError($"Provider course locations not found for ukprn: {request.Ukprn} ProviderCourseId: {request.ProviderCourseId}");
                return null;
            }

            List<ProviderCourseLocationModel> locations = new List<ProviderCourseLocationModel>();
            foreach (var location in providerCourseLocations)
            {
                var providerCourseLocationModel = new ProviderCourseLocationModel
                {
                    LocationName = location.LocationName,
                    LocationType = location.LocationType,
                    HasBlockReleaseDeliveryOption = location.HasBlockReleaseDeliveryOption,
                    HasDayReleaseDeliveryOption = location.HasDayReleaseDeliveryOption,
                    OffersPortableFlexiJob = location.OffersPortableFlexiJob
                };
                locations.Add(providerCourseLocationModel);
            }
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
                ProviderCourseLocations = locations
            };
        }
    }
}
