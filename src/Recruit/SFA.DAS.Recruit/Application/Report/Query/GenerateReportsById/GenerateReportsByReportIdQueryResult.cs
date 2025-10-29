using System.Collections.Generic;
using SFA.DAS.Recruit.Domain.Reports;

namespace SFA.DAS.Recruit.Application.Report.Query.GenerateReportsById;
public record GenerateReportsByReportIdQueryResult
{
    public List<ApplicationSummaryReport> Reports { get; set; }    
}