using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitQa.InnerApi.Requests;

public record GetGenerateQaReportRequest(Guid ReportId) : IGetApiRequest
{
    public string GetUrl => $"api/reports/generate-qa/{ReportId}";
}