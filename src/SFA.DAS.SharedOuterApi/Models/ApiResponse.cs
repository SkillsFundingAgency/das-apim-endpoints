using System.Net;

namespace SFA.DAS.SharedOuterApi.Models
{
    public class ApiResponse<TResponse>
    {
        public TResponse Body { get;  }
        public HttpStatusCode StatusCode { get; }

        public ApiResponse (TResponse body, HttpStatusCode statusCode)
        {
            Body = body;
            StatusCode = statusCode;
        }
    }
}