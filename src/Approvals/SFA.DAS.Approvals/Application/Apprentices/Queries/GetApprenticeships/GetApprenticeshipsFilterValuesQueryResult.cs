using System;
using System.Collections.Generic;

namespace SFA.DAS.Approvals.Application.Apprentices.Queries.GetApprenticeships;

public class GetApprenticeshipsFilterValuesQueryResult
{
    public IEnumerable<string> EmployerNames { get; set; }
    public IEnumerable<string> ProviderNames { get; set; }
    public IEnumerable<string> CourseNames { get; set; }
    public IEnumerable<DateTime> StartDates { get; set; }
    public IEnumerable<DateTime> EndDates { get; set; }
    public IEnumerable<string> Sectors { get; set; }
}