using MediatR;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ApprenticeFeedback.Application.Commands.CreateExitSurvey
{
    public class CreateExitSurveyCommand : IRequest<CreateExitSurveyResponse>
    {
        public Guid ApprenticeFeedbackTargetId { get; set; }
        public bool AllowContact { get; set; }
        public List<int> AttributeIds { get; set; }
        public int PrimaryReason { get; set; }
    }
}
