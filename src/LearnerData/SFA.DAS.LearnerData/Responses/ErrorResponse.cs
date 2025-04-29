namespace SFA.DAS.LearnerData.Responses;

public class ErrorResponse
{
    public IEnumerable<Error> Errors { get; set; }
}

public class Error
{
    public string Code { get; set; }
    public string Message { get; set; }
}