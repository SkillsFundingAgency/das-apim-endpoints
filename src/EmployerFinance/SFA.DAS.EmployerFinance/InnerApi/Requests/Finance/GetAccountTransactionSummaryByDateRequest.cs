using SFA.DAS.Apim.Shared.Interfaces;
using System;

namespace SFA.DAS.EmployerFinance.InnerApi.Requests.Finance;

public sealed record GetAccountTransactionSummaryByDateRequest(long AccountId, DateTime FromDate, DateTime ToDate) : IGetApiRequest
{
    public string GetUrl => $"api/accounts/{AccountId}/transaction-summary?fromDate={FromDate:yyyy-MM-dd}&toDate={ToDate:yyyy-MM-dd}";
}