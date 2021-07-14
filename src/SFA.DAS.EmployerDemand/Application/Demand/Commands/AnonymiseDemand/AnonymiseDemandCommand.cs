using System;
using MediatR;

namespace SFA.DAS.EmployerDemand.Application.Demand.Commands.AnonymiseDemand
{
    public class AnonymiseDemandCommand : IRequest<Unit>
    {
        public Guid EmployerDemandId { get; set; }
    }
}