using System.Net;

namespace SFA.DAS.EmployerIncentives.Infrastructure.Api
{
    public class InnerApiResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Content { get; set; }
    }
}