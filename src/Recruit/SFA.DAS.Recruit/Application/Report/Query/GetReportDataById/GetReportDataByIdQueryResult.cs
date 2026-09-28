using SFA.DAS.Recruit.Domain.Reports;
using System.Collections.Generic;

namespace SFA.DAS.Recruit.Application.Report.Query.GetReportDataById;

public record GetReportDataByIdQueryResult
{
    public List<ApplicationSummaryReport> Reports { get; set; } = [];
}
