using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerDemand.InnerApi.Responses;

namespace SFA.DAS.EmployerDemand.Application.Demand.Commands.StopEmployerDemand
{
    public class StopEmployerDemandCommand : IRequest<StopEmployerDemandCommandResult>
    {
        public Guid EmployerDemandId { get; set; }
    }

    public class StopEmployerDemandCommandHandler : IRequestHandler<StopEmployerDemandCommand, StopEmployerDemandCommandResult> {
        public async Task<StopEmployerDemandCommandResult> Handle(StopEmployerDemandCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }

    public class StopEmployerDemandCommandResult
    {
        public GetEmployerDemandResponse EmployerDemand { get; set; }
    }
}

