namespace SFA.DAS.AODP.Application;

public abstract class BaseResponse
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
}