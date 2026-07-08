using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests;

public class GetAllChangeHistoryForEmployerRequest(long accountId) : IGetApiRequest
{
    public long AccountId { get; } = accountId;
    public string GetUrl => $"api/change-history/employer/{AccountId}/get-all-change-history";
}