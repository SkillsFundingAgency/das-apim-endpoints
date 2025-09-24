using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipTraining.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.AccessorService;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RoatpV2;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.AccessorService;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseProvider;

public sealed class GetCourseProviderQueryHandler(
    IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _roatpCourseManagementApiClient,
    IAssessorsApiClient<AssessorsApiConfiguration> _assessorServiceInnerApiClient,
    ICachedLocationLookupService _cachedLocationLookupService,
    ICachedFeedbackService _cachedFeedbackService
) : IRequestHandler<GetCourseProviderQuery, GetCourseProviderQueryResult>
{
    public async Task<GetCourseProviderQueryResult> Handle(GetCourseProviderQuery query, CancellationToken cancellationToken)
    {
        LocationItem locationItem = await _cachedLocationLookupService.GetCachedLocationInformation(query.Location);

        List<Task> tasks = new List<Task>();

        var courseProviderDetailsResponse = await _roatpCourseManagementApiClient.GetWithResponseCode<GetCourseProviderDetailsResponse>(
            new GetCourseProviderDetailsRequest(
                query.LarsCode,
                query.Ukprn,
                query.Location,
                locationItem?.Longitude,
                locationItem?.Latitude,
                query.ShortlistUserId
            )
        );

        if (courseProviderDetailsResponse.StatusCode == HttpStatusCode.NotFound)
        {
            // If the course provider details are not found, return null so a not found response can be returned
            return null;
        }
        // Any other failure should throw an exception
        courseProviderDetailsResponse.EnsureSuccessStatusCode();

        var feedbackTask = _cachedFeedbackService.GetProviderFeedback(query.Ukprn);

        tasks.Add(feedbackTask);

        var courseDetailsTask = _roatpCourseManagementApiClient.GetWithResponseCode<List<ProviderCourseResponse>>(
            new ProviderCoursesRequest(query.Ukprn)
        );

        tasks.Add(courseDetailsTask);

        var courseTrainingProvidersCountTask = _roatpCourseManagementApiClient.GetWithResponseCode<GetCourseTrainingProvidersCountResponse>(
            new GetCourseTrainingProvidersCountRequest(
                [query.LarsCode],
                query.Distance,
                locationItem?.Latitude,
                locationItem?.Longitude
            )
        );
        tasks.Add(courseTrainingProvidersCountTask);

        await Task.WhenAll(tasks);

        ApiResponse<List<ProviderCourseResponse>> courseDetailsResponse = courseDetailsTask.Result;
        ApiResponse<GetCourseTrainingProvidersCountResponse> courseTrainingProvidersCountResponse = courseTrainingProvidersCountTask.Result;

        courseDetailsResponse.EnsureSuccessStatusCode();
        courseTrainingProvidersCountResponse.EnsureSuccessStatusCode();

        var assessmentsResponse =
            await _assessorServiceInnerApiClient.GetWithResponseCode<GetAssessmentsResponse>(
                new GetAssessmentsRequest(
                    query.Ukprn,
                    courseProviderDetailsResponse.Body.IFateReferenceNumber
                )
        );

        assessmentsResponse.EnsureSuccessStatusCode();

        CourseTrainingProviderCountModel trainingCourseCountDetails =
            courseTrainingProvidersCountResponse.Body.Courses.Count > 0 ?
                courseTrainingProvidersCountResponse.Body.Courses[0] :
                null;

        var (employerFB, apprenticeFB) = feedbackTask.Result;

        GetCourseProviderQueryResult result = courseProviderDetailsResponse.Body;
        result.TotalProvidersCount = trainingCourseCountDetails?.TotalProvidersCount ?? 0;
        result.Courses = courseDetailsResponse.Body.Select(a => (ProviderCourseModel)a);
        result.AnnualEmployerFeedbackDetails = employerFB.AnnualEmployerFeedbackDetails;
        result.AnnualApprenticeFeedbackDetails = apprenticeFB.AnnualApprenticeFeedbackDetails;
        result.EndpointAssessments = assessmentsResponse.Body;
        return result;
    }
}
