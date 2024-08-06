using MediatR;
using System;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetEmployerRequest
{
    public class GetEmployerRequestQuery : IRequest<GetEmployerRequestResult>
    {
        public Guid? EmployerRequestId { get; set; }
        public long? AccountId { get; set; }
        public string StandardReference { get; set; }
    }
}
