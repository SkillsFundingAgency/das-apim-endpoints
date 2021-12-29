using System.Net;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Pledges
{
    public class GetClosePledgeResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string ErrorContent { get; set; }
    }
}
