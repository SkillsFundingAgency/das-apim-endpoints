using MediatR;
using SFA.DAS.EmployerFeedback.Models;

namespace SFA.DAS.EmployerFeedback.Application.Commands.UpdateFeedbackTransaction
{
    public class UpdateFeedbackTransactionCommand : IRequest
    {
        public long Id { get; set; }
        public UpdateFeedbackTransactionRequest Request { get; set; }

        public UpdateFeedbackTransactionCommand(long id, UpdateFeedbackTransactionRequest request)
        {
            Id = id;
            Request = request;
        }
    }
}
