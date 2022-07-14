using System;
using MediatR;

namespace SFA.DAS.EmployerDemand.Application.Demand.Queries.GetRestartEmployerDemand
{
    public class GetRestartEmployerDemandQuery : IRequest<GetRestartEmployerDemandQueryResult>
    {
        public Guid Id { get ; set ; }
    }
}