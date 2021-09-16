using System.Net;

namespace SFA.DAS.LevyTransferMatching.Extensions
{
    public static class HttpStatusCodeExtensions
    {
        public static bool IsSuccess(this HttpStatusCode statusCode)
        {
            return ((int)statusCode >= 200) && ((int)statusCode <300); 
        }
    }
}
