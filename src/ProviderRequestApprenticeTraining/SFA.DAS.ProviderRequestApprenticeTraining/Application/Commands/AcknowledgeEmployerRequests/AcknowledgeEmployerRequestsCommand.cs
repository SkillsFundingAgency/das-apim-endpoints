using System.Collections.Generic;
using System;
using MediatR;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Commands.AcknowledgeEmployerRequests
{
    public class AcknowledgeEmployerRequestsCommand : IRequest
    {
        public long Ukprn { get; set; }
        public List<Guid> EmployerRequestIds { get; set; }
    }
}
