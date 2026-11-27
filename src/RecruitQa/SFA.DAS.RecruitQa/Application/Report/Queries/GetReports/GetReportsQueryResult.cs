using ReportModel = SFA.DAS.RecruitQa.Domain.Reports.Report;

namespace SFA.DAS.RecruitQa.Application.Report.Queries.GetReports;

public record GetReportsQueryResult
{
    public List<ReportModel> Reports { get; set; } = [];
}
