using System.Net.Http;

namespace SFA.DAS.SharedOuterApi.Infrastructure
{
    public static class HttpRequestMessageExtensions
    {
        public static void AddVersion(this HttpRequestMessage httpRequestMessage, string version)
        {
            httpRequestMessage.Headers.Add("X-Version", version);
        }
    }
}