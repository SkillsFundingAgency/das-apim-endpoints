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

        var courseProviderDetailsResponse =
            await _roatpCourseManagementApiClient.GetWithResponseCode<GetCourseProviderDetailsResponse>(
                new GetCourseProviderDetailsRequest(
                    query.LarsCode,
                    query.Ukprn,
                    query.Location,
                    locationItem?.Longitude,
                    locationItem?.Latitude,
                    query.ShortlistUserId
                )
        );

        courseProviderDetailsResponse.EnsureSuccessStatusCode();

        var assessmentsResponse =
            await _assessorServiceInnerApiClient.GetWithResponseCode<GetAssessmentsResponse>(
                new GetAssessmentsRequest(
                    query.Ukprn,
                    courseProviderDetailsResponse.Body.IFateReferenceNumber
                )
        );

        assessmentsResponse.EnsureSuccessStatusCode();

        var employerFeedbackResponse = 
            await _employerFeedbackApiClient.GetWithResponseCode<GetEmployerFeedbackSummaryAnnualResponse>(
                new GetEmployerFeedbackSummaryAnnualRequest(
                    query.Ukprn
                )
        );

        employerFeedbackResponse.EnsureSuccessStatusCode();

        var apprenticeFeedbackResponse = 
            await _apprenticeFeedbackApiClient.GetWithResponseCode<GetApprenticeFeedbackSummaryAnnualResponse>(
                new GetApprenticeFeedbackSummaryAnnualRequest(
                    query.Ukprn
                )
        );

        apprenticeFeedbackResponse.EnsureSuccessStatusCode();

        var courseDetailsResponse =
            await _roatpCourseManagementApiClient.GetWithResponseCode<List<ProviderCourseResponse>>(
                new ProviderCoursesRequest(
                    query.Ukprn    
                )
        );

        courseDetailsResponse.EnsureSuccessStatusCode();

        var courseTrainingProvidersCountResponse =
            await _roatpCourseManagementApiClient.GetWithResponseCode<GetCourseTrainingProvidersCountResponse>(
                new GetCourseTrainingProvidersCountRequest(
                    [query.LarsCode], 
                    query.Distance, 
                    locationItem?.Latitude, 
                    locationItem?.Longitude
                )
        );

        courseTrainingProvidersCountResponse.EnsureSuccessStatusCode();

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
