using System;
using MediatR;

namespace SFA.DAS.EmployerDemand.Application.Demand.Commands.SendAutomaticEmployerDemandDemandCutOff
{
    public class SendAutomaticEmployerDemandDemandCutOffCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public Guid EmployerDemandId { get; set; }
    }
}