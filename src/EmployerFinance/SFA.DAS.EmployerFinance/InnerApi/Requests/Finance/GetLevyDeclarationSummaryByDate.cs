using SFA.DAS.Apim.Shared.Interfaces;
using System;

namespace SFA.DAS.EmployerFinance.InnerApi.Requests.Finance;

public sealed record GetLevyDeclarationSummaryByDate(long AccountId, DateTime FromDate, DateTime ToDate) : IGetApiRequest
{
    public string GetUrl => $"api/levy-declarations/{AccountId}/summaryByDate?fromDate={FromDate:yyyy-MM-dd}&toDate={ToDate:yyyy-MM-dd}";
}