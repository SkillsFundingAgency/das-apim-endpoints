using MediatR;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetEmployerRequestsByIds
{
    public class GetEmployerRequestsByIdsQuery : IRequest<GetEmployerRequestsByIdsResult>
    {
        public List<Guid> EmployerRequestIds { get; set; }
    }
}
