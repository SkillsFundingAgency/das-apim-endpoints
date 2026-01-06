using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipTraining.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Assessor;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Assessor;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Providers.GetProviderSummary;

public class GetProviderSummaryQueryHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _roatpCourseManagementApiClient,
     IAssessorsApiClient<AssessorsApiConfiguration> _assessorsApiClient,
     ICachedFeedbackService _cachedFeedbackService
     ) : IRequestHandler<GetProviderSummaryQuery, GetProviderSummaryQueryResult>
{
    public async Task<GetProviderSummaryQueryResult> Handle(GetProviderSummaryQuery query, CancellationToken cancellationToken)
    {
        var providerSummaryResponseTask = _roatpCourseManagementApiClient.Get<GetProviderSummaryQueryResponse>(
                new GetProviderSummaryRequest(query.Ukprn));

        var providerCourseDetailsTask = _roatpCourseManagementApiClient.Get<List<GetProviderAdditionalStandardsItem>>(
            new GetProviderAdditionalStandardsRequest(query.Ukprn));

        var assessmentDetailsResponseTask = _assessorsApiClient.Get<GetEndpointAssessmentsResponse>(new GetEndpointAssessmentsRequest(query.Ukprn));

        var feedbackTask = _cachedFeedbackService.GetProviderFeedback(query.Ukprn);

        await Task.WhenAll(providerSummaryResponseTask, providerCourseDetailsTask, assessmentDetailsResponseTask, feedbackTask);

        var courses = new List<CourseDetails>();
        foreach (var course in providerCourseDetailsTask.Result)
        {
            courses.Add(new CourseDetails
            {
                CourseName = course.CourseName,
                Level = course.Level,
                LarsCode = course.LarsCode.ToString(),
                IfateReferenceNumber = course.IfateReferenceNumber
            });
        }

        var providerSummary = providerSummaryResponseTask.Result;
        var assessmentDetailsResponse = assessmentDetailsResponseTask.Result;
        var (employerFeedback, apprenticeFeedback) = feedbackTask.Result;

        var result = new GetProviderSummaryQueryResult
        {
            Ukprn = providerSummary.Ukprn,
            ProviderName = providerSummary.Name,
            ProviderAddress = providerSummary.Address,
            Contact = new ContactDetails
            {
                MarketingInfo = providerSummary.MarketingInfo,
                Email = providerSummary.Email,
                PhoneNumber = providerSummary.Phone,
                Website = providerSummary.ContactUrl
            },
            Qar = providerSummary.Qar,
            Reviews = providerSummary.Reviews,
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
