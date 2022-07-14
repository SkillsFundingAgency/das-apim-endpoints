using System.Net;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.CreditPledge
{
    public class CreditPledgeCommandResult
    {
        public bool CreditPledgeSkipped { get; set; }
        public string ErrorContent { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}