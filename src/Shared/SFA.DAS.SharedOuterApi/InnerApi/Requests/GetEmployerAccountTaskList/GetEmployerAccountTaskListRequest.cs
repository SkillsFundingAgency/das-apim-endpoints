using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.GetEmployerAccountTaskList;

public record GetEmployerAccountTaskListRequest(long AccountId, string HashedAccountId) : IGetApiRequest
{
    public string GetUrl => $"accounts/{AccountId}/account-task-list?hashedAccountId={HashedAccountId}";
}