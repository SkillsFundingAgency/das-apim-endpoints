using System.Collections.Generic;

namespace SFA.DAS.Approvals.InnerApi.EmploymentCheckApi.Responses;

public class GetEmploymentChecksResponse
{
    public IReadOnlyList<EvsCheckResponse> Checks { get; set; }
}
