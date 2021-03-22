using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.EmployerDemand.Application.Demand.Commands.RegisterDemand
{
    public class RegisterDemandCommandHandler : IRequestHandler<RegisterDemandCommand, RegisterDemandResult>
    {
        public async Task<RegisterDemandResult> Handle(RegisterDemandCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}