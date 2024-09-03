using System.Collections.Generic;
using System;
using MediatR;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Commands.CreateProviderResponseEmployerRequest
{
    public class CreateProviderResponseEmployerRequestCommand : IRequest<Unit>
    {
        public long Ukprn { get; set; }
        public List<Guid> EmployerRequestIds { get; set; }
    }
}
