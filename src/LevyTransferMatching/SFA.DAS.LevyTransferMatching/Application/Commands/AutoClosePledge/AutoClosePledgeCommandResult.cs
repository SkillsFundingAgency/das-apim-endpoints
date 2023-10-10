using MediatR;
using System.Net;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.AutoClosePledge
{
    public  class AutoClosePledgeCommandResult
    {
        public bool PledgeClosed { get; set; }
    }
}