namespace SFA.DAS.Recruit.Api.Models;

public class DataResponse<T>
{
    public required T Data { get; init; }
}