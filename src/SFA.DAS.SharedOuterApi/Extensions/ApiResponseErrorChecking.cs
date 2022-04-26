using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.Models;
using System;
using System.Net;

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
                throw ApiResponseException.Create(response);
            }

            return response;
        }

        public static bool IsSuccessStatusCode(HttpStatusCode statusCode)
            => (int)statusCode >= 200 && (int)statusCode <= 299;
    }
}