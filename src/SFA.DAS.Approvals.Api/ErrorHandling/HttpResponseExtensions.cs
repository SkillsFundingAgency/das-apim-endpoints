using System.Net;
using Microsoft.AspNetCore.Http;

namespace SFA.DAS.Approvals.ErrorHandling
{
    public static class HttpResponseExtensions
    {
        public static void SetStatusCode(this HttpResponse httpResponse, HttpStatusCode httpStatusCode)
        {
            httpResponse.StatusCode = (int)httpStatusCode;
        }

        public static void SetSubStatusCode(this HttpResponse httpResponse, HttpSubStatusCode httpSubStatusCode)
        {
            httpResponse.Headers[HttpHeaderNames.SubStatusCode] = ((int)httpSubStatusCode).ToString();
        }
    }
}
