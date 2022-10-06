using System.Net;

namespace SFA.DAS.TrackProgress.Application.Services;

public class TrackProgressApiException : Exception
{
    public TrackProgressApiException(HttpStatusCode statusCode, string details) : base(details)
    {
        StatusCode = statusCode;
    }

    public HttpStatusCode StatusCode { get; }
}