using SFA.DAS.Recruit.Domain.Reports;
using System.Collections.Generic;

namespace SFA.DAS.Recruit.InnerApi.Recruit.Responses.Reports;

public record GetReportDataResponse
{
    public List<ApplicationSummaryReport> Reports { get; set; } = [];
}
