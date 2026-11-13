using SFA.DAS.Apim.Shared.Interfaces;
using System;
using System.Collections.Generic;
using SFA.DAS.EmployerFinance.Models.Projections;

namespace SFA.DAS.EmployerFinance.InnerApi.Requests.Finance;

public sealed record PostLevyDeclarationSummaryByDate(long AccountId, DateTime FromDate, DateTime ToDate, List<string> Emprefs) : IPostApiRequest
{
    public string PostUrl => $"api/levy-declarations/{AccountId}/summaryByDate?fromDate={FromDate:yyyy-MM-dd}&toDate={ToDate:yyyy-MM-dd}";
    public object Data { get; set; } = Emprefs;
}