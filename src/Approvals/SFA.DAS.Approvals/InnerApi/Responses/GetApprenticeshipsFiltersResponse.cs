using System;
using System.Collections.Generic;

namespace SFA.DAS.Approvals.InnerApi.Responses;

public class GetApprenticeshipsFiltersResponse
{
    public IEnumerable<string> EmployerNames { get; set; }

    public IEnumerable<string> ProviderNames { get; set; }

    public IEnumerable<string> CourseNames { get; set; }

    public IEnumerable<string> Sectors { get; set; }

    public IEnumerable<DateTime> StartDates { get; set; }

    public IEnumerable<DateTime> EndDates { get; set; }
}