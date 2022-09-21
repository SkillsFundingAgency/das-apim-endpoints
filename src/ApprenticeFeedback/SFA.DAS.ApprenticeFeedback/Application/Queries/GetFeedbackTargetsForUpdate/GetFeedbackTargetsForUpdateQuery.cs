using MediatR;

namespace SFA.DAS.ApprenticeFeedback.Application.Queries.GetFeedbackTargetsForUpdate
{
    public class GetFeedbackTargetsForUpdateQuery : IRequest<GetFeedbackTargetsForUpdateResult>
    {
        public int BatchSize { get; set; }
    }
}
