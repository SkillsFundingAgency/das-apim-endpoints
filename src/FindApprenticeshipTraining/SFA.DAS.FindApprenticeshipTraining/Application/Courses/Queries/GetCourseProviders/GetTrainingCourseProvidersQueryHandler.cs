using MediatR;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseProviders
{
    public class GetTrainingCourseProvidersQueryHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _roatpCourseManagementApiClient, ILocationLookupService locationLookupService, ICoursesApiClient<CoursesApiConfiguration> coursesApiClient) : IRequestHandler<GetCourseProvidersQuery, GetCourseProvidersResponse>
    {

        public async Task<GetCourseProvidersResponse> Handle(GetCourseProvidersQuery request,
            CancellationToken cancellationToken)
        {
            decimal? latitude = null;
            decimal? longitude = null;

            if (request.Location != null)
            {
                var location = await locationLookupService.GetLocationInformation(request.Location, 0, 0);

                if (location == null)
                {
                    GetStandardsListItem standard =
                        await coursesApiClient.Get<GetStandardsListItem>(new GetStandardRequest(request.Id));

                    var standardName = standard != null ? $"{standard.Title} (level {standard.Level})" : "";

                    var responseToSendBack = new GetCourseProvidersResponse
                    {
                        PageSize = 10,
                        Page = 1,
                        LarsCode = request.Id,
                        Providers = new List<ProviderData>(),
                        QarPeriod = string.Empty,
                        ReviewPeriod = string.Empty,
                        StandardName = standardName,
                        TotalCount = 0,
                        TotalPages = 0
                    };

                    return responseToSendBack;
                }

                latitude = (decimal)location.GeoPoint[0];
                longitude = (decimal)location.GeoPoint[1];
            }

            var response =
                await _roatpCourseManagementApiClient.GetWithResponseCode<GetCourseProvidersResponse>(
                    new GetProvidersByCourseIdRequest()
                    {
                        CourseId = request.Id,
                        OrderBy = request.OrderBy,
                        Distance = request.Distance,
                        Latitude = latitude,
                        Longitude = longitude,
                        Location = request.Location,
                        DeliveryModes = request.DeliveryModes,
                        EmployerProviderRatings = request.EmployerProviderRatings,
                        ApprenticeProviderRatings = request.ApprenticeProviderRatings,
                        Qar = request.Qar,
                        Page = request.Page,
                        PageSize = request.PageSize,
                        UserId = request.ShortlistUserId
                    });

            return ApiResponseErrorChecking.IsSuccessStatusCode(response.StatusCode)
                    ? response.Body
                    : new GetCourseProvidersResponse();
        }
    }
}