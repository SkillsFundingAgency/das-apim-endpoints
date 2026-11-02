using MediatR;

namespace SFA.DAS.ApprenticeFeedback.Application.Commands.CreateFeedbackTransaction
{
    public class CreateFeedbackTransactionCommand : IRequest<CreateFeedbackTransactionResponse>
    {
        public long ApprenticeshipId { get; set; }
        public string TemplateName { get; set; }
    }
}
