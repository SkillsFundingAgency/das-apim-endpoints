using System.Net;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.DebitPledge
{
    public class DebitPledgeCommandResult
    {
        public HttpStatusCode StatusCode { get; set; }
        public string ErrorContent { get; set; }
    }
}