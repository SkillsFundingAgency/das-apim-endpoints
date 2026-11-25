using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;

public class GetTransferFinancialBreakdownRequest(long accountId, DateTime startDate) : IGetApiRequest
{
    public long AccountId { get; } = accountId;

    public string GetUrl => $"api/accounts/{AccountId}/accountprojection/detail?numberOfMonths=12&startDate={startDate.StartOfAprilOfFinancialYear().ToString("yyyy-MM-dd")}";
}