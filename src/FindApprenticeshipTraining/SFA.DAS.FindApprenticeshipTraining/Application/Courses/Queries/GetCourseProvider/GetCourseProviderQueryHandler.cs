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
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseProvider;

public sealed class GetCourseProviderQueryHandler(
    IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _roatpCourseManagementApiClient,
    IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration> _employerFeedbackApiClient,
    IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> _apprenticeFeedbackApiClient,
    IAssessorsApiClient<AssessorsApiConfiguration> _assessorServiceInnerApiClient,
    ICachedLocationLookupService _cachedLocationLookupService
) : IRequestHandler<GetCourseProviderQuery, GetCourseProviderQueryResult>
{
    public async Task<GetCourseProviderQueryResult> Handle(GetCourseProviderQuery query, CancellationToken cancellationToken)
    {
        LocationItem locationItem = await _cachedLocationLookupService.GetCachedLocationInformation(query.Location);

        List<Task> tasks = new List<Task>();

        var courseProviderDetailsTask = _roatpCourseManagementApiClient.GetWithResponseCode<GetCourseProviderDetailsResponse>(
            new GetCourseProviderDetailsRequest(
                query.LarsCode,
                query.Ukprn,
                query.Location,
                locationItem?.Longitude,
                locationItem?.Latitude,
                query.ShortlistUserId
            )
        );

        tasks.Add(courseProviderDetailsTask);

        var employerFeedbackTask = _employerFeedbackApiClient.GetWithResponseCode<GetEmployerFeedbackSummaryAnnualResponse>(
            new GetEmployerFeedbackSummaryAnnualRequest(query.Ukprn)
        );

        tasks.Add(employerFeedbackTask);

        var apprenticeFeedbackTask = _apprenticeFeedbackApiClient.GetWithResponseCode<GetApprenticeFeedbackSummaryAnnualResponse>(
            new GetApprenticeFeedbackSummaryAnnualRequest(query.Ukprn)
        );

        tasks.Add(apprenticeFeedbackTask);

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

        ApiResponse<GetCourseProviderDetailsResponse> courseProviderDetailsResponse = courseProviderDetailsTask.Result;
        ApiResponse<GetEmployerFeedbackSummaryAnnualResponse> employerFeedbackResponse = employerFeedbackTask.Result;
        ApiResponse<GetApprenticeFeedbackSummaryAnnualResponse> apprenticeFeedbackResponse = apprenticeFeedbackTask.Result;
        ApiResponse<List<ProviderCourseResponse>> courseDetailsResponse = courseDetailsTask.Result;
        ApiResponse<GetCourseTrainingProvidersCountResponse> courseTrainingProvidersCountResponse = courseTrainingProvidersCountTask.Result;

        if (courseProviderDetailsResponse.StatusCode == HttpStatusCode.NotFound)
            return null;

        courseProviderDetailsResponse.EnsureSuccessStatusCode();
        employerFeedbackResponse.EnsureSuccessStatusCode();
        apprenticeFeedbackResponse.EnsureSuccessStatusCode();
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

        GetCourseProviderQueryResult result = courseProviderDetailsResponse.Body;
        result.TotalProvidersCount = trainingCourseCountDetails?.TotalProvidersCount ?? 0;
        result.Courses = courseDetailsResponse.Body.Select(a => (ProviderCourseModel)a);
        result.AnnualEmployerFeedbackDetails = employerFeedbackResponse.Body.AnnualEmployerFeedbackDetails;
        result.AnnualApprenticeFeedbackDetails = apprenticeFeedbackResponse.Body.AnnualApprenticeFeedbackDetails;
        result.EndpointAssessments = assessmentsResponse.Body;
        return result;
    }
}
