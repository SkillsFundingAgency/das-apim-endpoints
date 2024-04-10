using MediatR;
using System;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetEmployerRequest
{
    public class GetEmployerRequestQuery : IRequest<GetEmployerRequestResult>
    {
        public Guid EmployerRequestId { get; set; }
    }
}
