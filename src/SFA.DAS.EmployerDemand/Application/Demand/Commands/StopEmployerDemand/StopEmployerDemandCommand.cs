using System;
using MediatR;

namespace SFA.DAS.EmployerDemand.Application.Demand.Commands.StopEmployerDemand
{
    public class StopEmployerDemandCommand : IRequest<StopEmployerDemandCommandResult>
    {
        public Guid Id { get; set; }
        public Guid EmployerDemandId { get; set; }
    }
}
