using System.Net;

public class CreateMetricsDataCommandResult
{
    public HttpStatusCode StatusCode { get; set;  }
    public string? ErrorMessage { get; set; }
}