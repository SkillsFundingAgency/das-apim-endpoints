using MediatR;

namespace SFA.DAS.ApprenticeFeedback.Application.Commands.CreateOutcomeRecordedFeedbackTransaction
{
    public class CreateOutcomeRecordedFeedbackTransactionCommand : IRequest<CreateOutcomeRecordedFeedbackTransactionResponse>
    {
        public long ApprenticeshipId { get; set; }
    }
}
