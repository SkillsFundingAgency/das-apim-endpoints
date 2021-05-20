using System;
using MediatR;

namespace SFA.DAS.EmployerDemand.Application.Demand.Commands.VerifyEmployerDemand
{
    public class VerifyEmployerDemandCommand : IRequest<VerifyEmployerDemandCommandResult>
    {
        public Guid Id { get ; set ; }
    }
}