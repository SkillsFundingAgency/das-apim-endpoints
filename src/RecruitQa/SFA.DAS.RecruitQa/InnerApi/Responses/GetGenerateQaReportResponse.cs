using SFA.DAS.RecruitQa.Domain.Reports;

namespace SFA.DAS.RecruitQa.InnerApi.Responses;

public record GetGenerateQaReportResponse
{
    public List<QaReport> QaReports { get; set; } = [];
}
