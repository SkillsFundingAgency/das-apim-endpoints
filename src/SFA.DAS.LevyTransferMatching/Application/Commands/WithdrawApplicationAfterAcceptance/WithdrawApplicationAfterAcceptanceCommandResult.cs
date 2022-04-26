using System.Net;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.WithdrawApplicationAfterAcceptance
{
    public class WithdrawApplicationAfterAcceptanceCommandResult
    {
        public HttpStatusCode StatusCode { get; set; }
        public string ErrorContent { get; set; }
    }
}
