using System.Text.Json.Serialization;
public class BaseMediatrResponse<T> where T : class, new()
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public string? ErrorCode { get; set; }
    public T Value { get; set; } = new();
}

public static class ErrorCodes
{
    public const string NoData = "NO_DATA";
    public const string UnexpectedError = "UNEXPECTED_ERROR";
}
