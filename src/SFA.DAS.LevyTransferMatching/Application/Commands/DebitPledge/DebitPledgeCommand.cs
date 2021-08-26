using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.DebitPledge
{
    public class DebitPledgeCommand : IRequest
    {
        public int PledgeId { get; set; }
        public int Amount { get; set; }
    }

    public class DebitPledgeCommandHandler : IRequestHandler<DebitPledgeCommand>
    {
        public DebitPledgeCommandHandler()
        {
            
        }

        public Task<Unit> Handle(DebitPledgeCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }



}
