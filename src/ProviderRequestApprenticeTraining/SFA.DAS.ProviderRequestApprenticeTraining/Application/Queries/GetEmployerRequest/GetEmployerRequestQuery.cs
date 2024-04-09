using MediatR;
using System;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetEmployerRequest
{
    public class GetEmployerRequestQuery : IRequest<GetEmployerRequestResult>
    {
        public Guid EmployerRequestId { get; set; }
    }
}
