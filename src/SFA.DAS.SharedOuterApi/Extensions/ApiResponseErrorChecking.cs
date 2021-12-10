using SFA.DAS.SharedOuterApi.Models;
using System;
using System.Net;
using System.Net.Http;

namespace SFA.DAS.SharedOuterApi.Extensions
{
    public static class ApiResponseErrorChecking
    {
        public static ApiResponse<T> EnsureSuccessStatusCode<T>(this ApiResponse<T> response)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            if (!IsSuccessStatusCode(response.StatusCode))
            {
                throw new HttpRequestException(
                    $"HTTP status code did not indicate success: {(int)response.StatusCode} {response.StatusCode}");
            }

            return response;
        }

        private static bool IsSuccessStatusCode(HttpStatusCode statusCode)
            => (int)statusCode >= 200 && (int)statusCode <= 299;
    }
}