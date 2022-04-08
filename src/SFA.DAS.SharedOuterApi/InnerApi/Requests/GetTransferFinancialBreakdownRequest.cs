using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
{
    public class GetTransferFinancialBreakdownRequest : IGetApiRequest
    {
        public long AccountId { get; }

        public GetTransferFinancialBreakdownRequest(long accountId)
        {
            AccountId = accountId;
        }

        public string GetUrl => $"forecasting-api/accounts/{AccountId}/accountprojection/detail";
    }
}
