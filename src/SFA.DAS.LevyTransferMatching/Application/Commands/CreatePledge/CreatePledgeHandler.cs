using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.CreatePledge
{
    public class CreatePledgeHandler : IRequestHandler<CreatePledgeCommand, CreatePledgeResult>
    {
        public Task<CreatePledgeResult> Handle(CreatePledgeCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}