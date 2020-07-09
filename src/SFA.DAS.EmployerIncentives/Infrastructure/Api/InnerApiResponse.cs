using System.Net;
using System.Text.Json;

namespace SFA.DAS.EmployerIncentives.Infrastructure.Api
{
    public class InnerApiResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public JsonDocument Json { get; set; }
    }
}