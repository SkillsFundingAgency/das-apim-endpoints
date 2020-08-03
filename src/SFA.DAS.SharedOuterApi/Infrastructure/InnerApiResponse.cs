using System.Net;
using System.Text.Json;

namespace SFA.DAS.SharedOuterApi.Infrastructure
{
    public class InnerApiResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public JsonDocument Json { get; set; }
    }
}