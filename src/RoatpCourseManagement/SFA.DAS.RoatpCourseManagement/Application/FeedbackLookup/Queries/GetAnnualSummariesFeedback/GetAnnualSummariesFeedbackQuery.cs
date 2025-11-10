using MediatR;

namespace SFA.DAS.RoatpCourseManagement.Application.FeedbackLookup.Queries.GetAnnualSummariesFeedback;

public record GetAnnualSummariesFeedbackQuery(string timePeriod) : IRequest<GetAnnualSummariesFeedbackQueryResult>;
