using System;
using System.Linq;
using System.Net.Http;

namespace SFA.DAS.Approvals.ErrorHandling
{
    public static class HttpResponseMessageExtensions
    {
        public static HttpSubStatusCode GetSubStatusCode(this HttpResponseMessage httpResponseMessage)
        {
            var httpSubStatusCode = HttpSubStatusCode.None;

            if (httpResponseMessage.Headers.TryGetValues(HttpHeaderNames.SubStatusCode, out var values))
            {
                var subStatusCodes = values.ToList();

                if (subStatusCodes.Count != 1 || !Enum.TryParse(subStatusCodes.Single(), out httpSubStatusCode))
                {
                    throw new InvalidOperationException($"HTTP response header {HttpHeaderNames.SubStatusCode} is invalid");
                }
            }

            return httpSubStatusCode;
        }
    }
}
