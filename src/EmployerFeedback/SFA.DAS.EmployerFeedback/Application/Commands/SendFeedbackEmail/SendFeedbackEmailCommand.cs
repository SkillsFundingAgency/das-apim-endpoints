using MediatR;
using SFA.DAS.EmployerFeedback.Models;

namespace SFA.DAS.EmployerFeedback.Application.Commands.SendFeedbackEmail
{
    public class SendFeedbackEmailCommand : IRequest
    {
        public SendFeedbackEmailCommand(SendFeedbackEmailRequest request)
        {
            Request = request;
        }

        public SendFeedbackEmailRequest Request { get; }
    }
}