using System.Collections.Generic;

namespace SFA.DAS.Recruit.Application.Report.Query.GetReportsByUkprn;
public record GetReportsByUkprnQueryResult(List<Domain.Reports.Report> Reports);