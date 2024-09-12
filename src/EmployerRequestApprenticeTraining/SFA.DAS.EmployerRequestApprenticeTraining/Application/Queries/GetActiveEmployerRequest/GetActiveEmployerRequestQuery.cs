using MediatR;
using System;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetActiveEmployerRequest
{
    public class GetActiveEmployerRequestQuery : IRequest<GetActiveEmployerRequestResult>
    {
        public long? AccountId { get; set; }
        public string StandardReference { get; set; }
    }
}
