using System.Collections.Generic;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.EmployerFinance.InnerApi.Responses;

namespace SFA.DAS.EmployerFinance.InnerApi.Requests.Finance;

public sealed record PostEmployerFundingProjectionRequest(long AccountId, PostEmployerFundingProjectionRequestData PostData) : IPostApiRequest
{
    public string PostUrl { get; } = $"api/employer/{AccountId}/funding-projection";
    public object Data { get; set; } = PostData;
}

public class PostEmployerFundingProjectionRequestData
{
    public int Months { get; set; } = 6;
    public List<LevyInMonthSummary> HistoricLevyIn { get; set; } = [];
}