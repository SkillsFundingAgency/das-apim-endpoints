namespace SFA.DAS.LearnerData.Responses;

public class ErrorResponse
{
    public List<Error> Errors { get; set; } = new();
}

public class Error
{
    public string Code { get; set; }
    public string Message { get; set; }
}