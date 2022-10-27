using MediatR;
using System;

namespace SFA.DAS.ApprenticeFeedback.Application.Queries.GetApprenticeFeedbackTargets
{
    public class GetApprenticeFeedbackTargetsQuery : IRequest<GetApprenticeFeedbackTargetsResult>
    {
        public Guid ApprenticeId { get; set; }
    }
}
