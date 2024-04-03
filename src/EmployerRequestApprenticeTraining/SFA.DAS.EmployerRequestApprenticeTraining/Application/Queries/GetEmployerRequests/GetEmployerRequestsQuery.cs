using MediatR;
using System;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetEmployerRequests
{
    public class GetEmployerRequestsQuery : IRequest<GetEmployerRequestsResult>
    {
        public long AccountId { get; set; }
    }
}
