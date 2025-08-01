using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ApprenticeFeedback;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerFeedback;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ApprenticeFeedback;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerFeedback;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.Application.FeedbackLookup.Queries.GetAnnualSummariesFeedback;

public class GetAnnualSummariesFeedbackQueryHandler(IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> _apprenticeFeedbackApiClient, IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration> _employerFeedbackApiClient) : IRequestHandler<GetAnnualSummariesFeedbackQuery, GetAnnualSummariesFeedbackQueryResult>
{
    public async Task<GetAnnualSummariesFeedbackQueryResult> Handle(GetAnnualSummariesFeedbackQuery request, CancellationToken cancellationToken)
    {
        var apprenticeReviewsTask = _apprenticeFeedbackApiClient.Get<IEnumerable<ApprenticeFeedbackStarsSummary>>(new GetApprenticeFeedbackAnnualReviewsRequest(request.timePeriod));

        var employerReviewsTask = _employerFeedbackApiClient.Get<IEnumerable<EmployerFeedbackStarsSummary>>(new GetEmployerFeedbackAnnualReviewsRequest(request.timePeriod));

        await Task.WhenAll(apprenticeReviewsTask, employerReviewsTask);

        return new GetAnnualSummariesFeedbackQueryResult(employerReviewsTask.Result, apprenticeReviewsTask.Result);
    }
}
