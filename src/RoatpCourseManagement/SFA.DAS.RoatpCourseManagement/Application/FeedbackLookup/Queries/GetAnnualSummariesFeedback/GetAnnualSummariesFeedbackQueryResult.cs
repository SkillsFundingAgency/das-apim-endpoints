using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ApprenticeFeedback;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerFeedback;

namespace SFA.DAS.RoatpCourseManagement.Application.FeedbackLookup.Queries.GetAnnualSummariesFeedback;

public record GetAnnualSummariesFeedbackQueryResult(IEnumerable<EmployerFeedbackStarsSummary> EmployersFeedback, IEnumerable<ApprenticeFeedbackStarsSummary> ApprenticesFeedback);
