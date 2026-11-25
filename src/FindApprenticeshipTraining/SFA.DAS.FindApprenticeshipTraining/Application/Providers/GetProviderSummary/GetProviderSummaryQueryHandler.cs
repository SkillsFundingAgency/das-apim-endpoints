using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipTraining.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Assessor;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Assessor;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Providers.GetProviderSummary;

public class GetProviderSummaryQueryHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _roatpCourseManagementApiClient,
     IAssessorsApiClient<AssessorsApiConfiguration> _assessorsApiClient,
     ICachedFeedbackService _cachedFeedbackService
     ) : IRequestHandler<GetProviderSummaryQuery, GetProviderSummaryQueryResult>
{
    public async Task<GetProviderSummaryQueryResult> Handle(GetProviderSummaryQuery query, CancellationToken cancellationToken)
    {
        var providerSummaryResponse = await _roatpCourseManagementApiClient.Get<GetProviderSummaryQueryResponse>(
                new GetProviderSummaryRequest(query.Ukprn));

        if (providerSummaryResponse == null)
        {
            return null;
        }

        var providerCourseDetailsTask = _roatpCourseManagementApiClient.Get<List<GetProviderAdditionalStandardsItem>>(
            new GetProviderAdditionalStandardsRequest(query.Ukprn));

        var assessmentDetailsResponseTask = _assessorsApiClient.Get<GetEndpointAssessmentsResponse>(new GetEndpointAssessmentsRequest(query.Ukprn));

        var feedbackTask = _cachedFeedbackService.GetProviderFeedback(query.Ukprn);

        await Task.WhenAll(providerCourseDetailsTask, assessmentDetailsResponseTask, feedbackTask);

        var courses = new List<CourseDetails>();

        var providerCourseDetails = providerCourseDetailsTask.Result ?? [];

        foreach (var course in providerCourseDetails)
        {
            courses.Add(new CourseDetails
            {
                CourseName = course.CourseName,
                CourseType = course.CourseType,
                LearningType = course.LearningType,
                Level = course.Level,
                LarsCode = course.LarsCode,
                IfateReferenceNumber = course.IfateReferenceNumber
            });
        }

        var assessmentDetailsResponse = assessmentDetailsResponseTask.Result;
        var (employerFeedback, apprenticeFeedback) = feedbackTask.Result;

        var result = new GetProviderSummaryQueryResult
        {
            Ukprn = providerSummaryResponse.Ukprn,
            ProviderName = providerSummaryResponse.Name,
            ProviderAddress = providerSummaryResponse.Address,
            Contact = new ContactDetails
            {
                MarketingInfo = providerSummaryResponse.MarketingInfo,
                Email = providerSummaryResponse.Email,
                PhoneNumber = providerSummaryResponse.Phone,
                Website = providerSummaryResponse.ContactUrl
            },
            Qar = providerSummaryResponse.Qar,
            Reviews = providerSummaryResponse.Reviews,
            Courses = courses,
            EndpointAssessments = new EndpointAssessmentsDetails
            {
                EarliestAssessment = assessmentDetailsResponse.EarliestAssessment,
                EndpointAssessmentCount = assessmentDetailsResponse.EndpointAssessmentCount
            },
            AnnualEmployerFeedbackDetails = employerFeedback.AnnualEmployerFeedbackDetails,
            AnnualApprenticeFeedbackDetails = apprenticeFeedback.AnnualApprenticeFeedbackDetails
        };

        return result;
    }
}
