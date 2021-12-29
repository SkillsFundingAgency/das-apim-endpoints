using System.Net;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.ClosePledge
{
    public class ClosePledgeCommandResult
    {
        public HttpStatusCode StatusCode { get; set; }
        public string ErrorContent { get; set; }
    }
}
