using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace SFA.DAS.TrackProgress.Application.Commands
{
    public class TrackProgressResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;

        public IActionResult result
            => StatusCode switch
            {
                HttpStatusCode.Created => new CreatedResult(string.Empty, null),
                HttpStatusCode.NotFound => new NotFoundObjectResult(Message),
                _ => new ObjectResult(new { StatusCode, Message }),
            };

        public TrackProgressResponse(HttpStatusCode statusCode)
            => StatusCode = statusCode;

        public TrackProgressResponse(HttpStatusCode statusCode, string message)
            => (StatusCode, Message) = (statusCode, message);
    }
}
