using MediatR;
using System;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.AcknowledgeProviderResponses
{
    public class AcknowledgeProviderResponsesCommand : IRequest
    {
        public Guid EmployerRequestId { get; set; }
        public Guid AcknowledgedBy { get; set; }
    }
}
