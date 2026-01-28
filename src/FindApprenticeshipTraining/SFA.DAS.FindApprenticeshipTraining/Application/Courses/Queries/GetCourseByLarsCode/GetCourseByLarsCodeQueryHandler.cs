using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindApprenticeshipTraining.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RoatpV2;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseByLarsCode;

public sealed class GetCourseByLarsCodeQueryHandler(
    ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient,
    IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _roatpCourseManagementApiClient,
    ICachedLocationLookupService _cachedLocationLookupService
) : IRequestHandler<GetCourseByLarsCodeQuery, GetCourseByLarsCodeQueryResult>
{

    public async Task<GetCourseByLarsCodeQueryResult> Handle(GetCourseByLarsCodeQuery query, CancellationToken cancellationToken)
    {
        LocationItem locationItem = await _cachedLocationLookupService.GetCachedLocationInformation(query.Location);

        var courseTrainingProvidersCountResponse =
            await _roatpCourseManagementApiClient.GetWithResponseCode<GetCourseTrainingProvidersCountResponse>(
                new GetCourseTrainingProvidersCountRequest(
                    [query.LarsCode],
                    query.Distance,
                    locationItem?.Latitude,
                    locationItem?.Longitude
                )
            );

        if (courseTrainingProvidersCountResponse.StatusCode is HttpStatusCode.BadRequest or HttpStatusCode.NotFound)
        {
            return null;
        }

        courseTrainingProvidersCountResponse.EnsureSuccessStatusCode();

        CourseTrainingProviderCountModel trainingCourseCountDetails =
            courseTrainingProvidersCountResponse.Body.Courses.Count > 0 ?
                courseTrainingProvidersCountResponse.Body.Courses[0] :
                null;

        var coursesApiStandardResponse = await _coursesApiClient.GetWithResponseCode<StandardDetailResponse>(
            new GetStandardDetailsByIdRequest(
                query.LarsCode
            )
        );

        coursesApiStandardResponse.EnsureSuccessStatusCode();

        StandardDetailResponse standardDetails = coursesApiStandardResponse.Body;

        ApprenticeshipFunding apprenticeshipFunding = standardDetails.ApprenticeshipFunding?.Count > 0 ?
            standardDetails.ApprenticeshipFunding.OrderByDescending(a => a.EffectiveFrom).First() :
        null;

        GetCourseByLarsCodeQueryResult result = standardDetails;

        result.MaxFunding = apprenticeshipFunding?.MaxEmployerLevyCap ?? 0;
        result.TypicalDuration = apprenticeshipFunding?.Duration ?? 0;

        result.ProvidersCountWithinDistance = trainingCourseCountDetails?.ProvidersCount ?? 0;
        result.TotalProvidersCount = trainingCourseCountDetails?.TotalProvidersCount ?? 0;
        result.IncentivePayment = CalculateIncentivePayment(apprenticeshipFunding);
        return result;
    }

    private static int CalculateIncentivePayment(ApprenticeshipFunding apprenticeshipFunding)
    {
        if (apprenticeshipFunding == null) return 0;

        return (apprenticeshipFunding.FoundationAppFirstEmpPayment ?? 0)
            + (apprenticeshipFunding.FoundationAppSecondEmpPayment ?? 0)
            + (apprenticeshipFunding.FoundationAppThirdEmpPayment ?? 0);
    }
}