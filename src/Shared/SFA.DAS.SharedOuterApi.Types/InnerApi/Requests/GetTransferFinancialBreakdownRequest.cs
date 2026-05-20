using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.Apim.Shared.Interfaces;

using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests
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
