using MediatR;
using System;

namespace SFA.DAS.ApprenticeFeedback.Application.Commands.CreateExitSurvey
{
    public class CreateExitSurveyCommand : IRequest<CreateExitSurveyResponse>
    {
        public Guid ApprenticeFeedbackTargetId { get; set; }
        public bool DidNotCompleteApprenticeship { get; set; }
        public string IncompletionReason { get; set; }
        // Incompletion factors:
        public bool IncompletionFactor_Caring { get; set; }
        public bool IncompletionFactor_Family { get; set; }
        public bool IncompletionFactor_Financial { get; set; }
        public bool IncompletionFactor_Mental { get; set; }
        public bool IncompletionFactor_Physical { get; set; }
        public bool IncompletionFactor_Other { get; set; }
        public string RemainedReason { get; set; }
        public string ReasonForIncorrect { get; set; }
        public bool AllowContact { get; set; }
    }
}
