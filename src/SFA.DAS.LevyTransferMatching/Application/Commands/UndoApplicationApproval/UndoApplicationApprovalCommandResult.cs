using System.Net;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.DebitPledge
{
    public class UndoApplicationApprovalResult
    {
        public HttpStatusCode StatusCode { get; set; }
        public string ErrorContent { get; set; }
    }
}