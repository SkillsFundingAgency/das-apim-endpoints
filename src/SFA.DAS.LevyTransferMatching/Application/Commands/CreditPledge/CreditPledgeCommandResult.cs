using System.Net;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.CreditPledge
{
    public class CreditPledgeCommandResult
    {
        public string ErrorContent { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}