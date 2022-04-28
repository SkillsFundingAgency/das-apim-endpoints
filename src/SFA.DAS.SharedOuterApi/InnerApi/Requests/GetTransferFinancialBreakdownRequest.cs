using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
{
    public class GetTransferFinancialBreakdownRequest : IGetApiRequest
    {
        public long AccountId { get; }        
        public GetTransferFinancialBreakdownRequest(long accountId)
        {
            AccountId = accountId;
        }
        public string GetUrl => $"accounts/{AccountId}/accountprojection/detail?numberOfMonths=12&startDate={DateTime.UtcNow.StartOfAprilOfFinancialYear().ToString("yyyy-MM-dd")}";
    }
}
