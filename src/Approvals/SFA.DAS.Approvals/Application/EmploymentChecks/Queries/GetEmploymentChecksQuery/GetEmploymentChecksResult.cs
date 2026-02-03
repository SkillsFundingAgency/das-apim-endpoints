using System.Collections.Generic;
using SFA.DAS.Approvals.InnerApi.EmploymentCheckApi.Responses;

namespace SFA.DAS.Approvals.Application.EmploymentChecks.Queries.GetEmploymentChecksQuery;

public class GetEmploymentChecksResult
{
    public IReadOnlyList<EvsCheckResponse> Checks { get; set; }
}
