using System.Net.Http;

namespace SFA.DAS.SharedOuterApi.Infrastructure
{
    public static class HttpRequestMessageExtensions
    {
        public static void AddVersion(this HttpRequestMessage request, string version)
        {
            request.Headers.Add("X-Version", version);
        }
    }
}