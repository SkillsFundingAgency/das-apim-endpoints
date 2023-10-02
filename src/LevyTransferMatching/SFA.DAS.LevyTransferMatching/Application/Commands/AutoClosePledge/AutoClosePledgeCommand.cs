using MediatR;
using System.Net;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.AutoClosePledge
{
    public  class AutoClosePledgeCommand : IRequest<HttpStatusCode>
    {
        public int PledgeId { get; set; }
        public int ApplicationId { get; set; }
    }
}