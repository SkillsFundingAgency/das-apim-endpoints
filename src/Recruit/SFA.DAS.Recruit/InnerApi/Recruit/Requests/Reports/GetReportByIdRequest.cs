using SFA.DAS.Apim.Shared.Interfaces;
using System;

namespace SFA.DAS.Recruit.InnerApi.Recruit.Requests.Reports;

public record GetReportByIdRequest(Guid ReportId) : IGetApiRequest
{
    public string GetUrl => $"api/reports/{ReportId}";
}