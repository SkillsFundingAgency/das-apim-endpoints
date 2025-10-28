using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.Recruit.InnerApi.Recruit.Requests.Reports;
public record GetGenerateReportRequest(Guid ReportId) : IGetApiRequest
{
    public string GetUrl => $"api/reports/{ReportId}";
}