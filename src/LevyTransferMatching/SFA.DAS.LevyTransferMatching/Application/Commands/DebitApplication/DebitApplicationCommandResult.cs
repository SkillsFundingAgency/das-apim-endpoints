using System.Net;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.DebitApplication
{
    public class DebitApplicationCommandResult
    {
        public string ErrorContent { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
