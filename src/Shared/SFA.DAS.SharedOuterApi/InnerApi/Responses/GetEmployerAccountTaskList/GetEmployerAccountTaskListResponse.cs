namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.GetEmployerAccountTaskList;

public record GetEmployerAccountTaskListResponse
{
    public bool HasProviders { get; set; }

    public bool HasPermissions { get; set; }
}