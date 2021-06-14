using System;
using MediatR;

namespace SFA.DAS.EmployerDemand.Application.Demand.Commands.StopEmployerDemand
{
    public class StopEmployerDemandCommand : IRequest<StopEmployerDemandCommandResult>
    {
        public Guid EmployerDemandId { get; set; }
    }
}
