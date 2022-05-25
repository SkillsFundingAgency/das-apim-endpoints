using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
{
    public class GetTransferFinancialBreakdownRequest : IGetApiRequest
    {
        private DateTime _startDate;
        public long AccountId { get; }
        
        public GetTransferFinancialBreakdownRequest(long accountId, DateTime startDate)
        {
            AccountId = accountId;
            _startDate = startDate;
        }
        public string GetUrl => $"api/accounts/{AccountId}/accountprojection/detail?numberOfMonths=12&startDate={_startDate.StartOfAprilOfFinancialYear().ToString("yyyy-MM-dd")}";
    }
}
