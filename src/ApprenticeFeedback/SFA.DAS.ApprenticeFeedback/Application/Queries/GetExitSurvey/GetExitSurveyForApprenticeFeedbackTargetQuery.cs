using MediatR;
using System;

namespace SFA.DAS.ApprenticeFeedback.Application.Queries.GetExitSurvey
{
    public class GetExitSurveyForApprenticeFeedbackTargetQuery : IRequest<GetExitSurveyForApprenticeFeedbackTargetResult>
    {
        public Guid ApprenticeFeedbackTargetId { get; set; }
    }
}
