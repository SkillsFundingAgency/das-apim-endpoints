using MediatR;

namespace SFA.DAS.EmployerFeedback.Application.Commands.UpsertFeedbackTransaction
{
    public class UpsertFeedbackTransactionCommand : IRequest
    {
        public long AccountId { get; set; }
    }
}