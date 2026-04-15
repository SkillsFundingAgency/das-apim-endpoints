using SFA.DAS.RecruitQa.Domain.Reports;

namespace SFA.DAS.RecruitQa.Application.Report.Queries.GenerateQaReport;

public record GenerateQaReportQueryResult
{
    public List<QaReportProjected> QaReports { get; set; } = [];
    public string? ReportName { get; set; }
}
