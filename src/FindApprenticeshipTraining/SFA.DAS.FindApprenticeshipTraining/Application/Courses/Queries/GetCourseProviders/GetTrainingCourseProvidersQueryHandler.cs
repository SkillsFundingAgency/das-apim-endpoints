using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipTraining.Services;
using SFA.DAS.SharedOuterApi.Common;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using GetStandardRequest = SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests.GetStandardRequest;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseProviders;

public class GetTrainingCourseProvidersQueryHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _roatpCourseManagementApiClient, ICachedLocationLookupService _cachedLocationLookupService, ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient) : IRequestHandler<GetCourseProvidersQuery, GetCourseProvidersResponse>
{
    public async Task<GetCourseProvidersResponse> Handle(GetCourseProvidersQuery request, CancellationToken cancellationToken)
    {
        LocationItem locationItem = null;

        if (!string.IsNullOrWhiteSpace(request.Location))
        {
            locationItem = await _cachedLocationLookupService.GetCachedLocationInformation(request.Location);

            if (locationItem is null)
            {
                GetStandardsListItem standard = await _coursesApiClient.Get<GetStandardsListItem>(new GetStandardRequest(request.LarsCode));

                var standardName = standard != null ? $"{standard.Title} (level {standard.Level})" : string.Empty;

                return new GetCourseProvidersResponse
                {
                    PageSize = 10,
                    Page = 1,
                    LarsCode = request.LarsCode,
                    Providers = [],
                    QarPeriod = string.Empty,
                    ReviewPeriod = string.Empty,
                    StandardName = standardName,
                    TotalCount = 0,
                    TotalPages = 0
                };
            }
        }

        var response =
            await _roatpCourseManagementApiClient.GetWithResponseCode<GetCourseProvidersResponseFromCourseApi>(
                new GetProvidersByCourseIdRequest()
                {
                    CourseId = request.LarsCode,
                    OrderBy = request.OrderBy,
                    Distance = request.Distance,
                    Latitude = locationItem?.Latitude,
                    Longitude = locationItem?.Longitude,
                    Location = request.Location,
                    DeliveryModes = request.DeliveryModes,
                    EmployerProviderRatings = request.EmployerProviderRatings,
                    ApprenticeProviderRatings = request.ApprenticeProviderRatings,
                    Qar = request.Qar,
                    Page = request.Page,
                    PageSize = request.PageSize,
                    UserId = request.ShortlistUserId
                }
            );

        if (response.StatusCode is HttpStatusCode.BadRequest or HttpStatusCode.NotFound)
            return null;

        return response.StatusCode.IsSuccessStatusCode()
            ? (GetCourseProvidersResponse)response.Body
            : new GetCourseProvidersResponse();
    }
}