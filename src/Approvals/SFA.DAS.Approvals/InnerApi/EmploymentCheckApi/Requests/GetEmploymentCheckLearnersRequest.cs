using System.Collections.Generic;
using System.Linq;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.EmploymentCheckApi.Requests;

public class GetEmploymentCheckLearnersRequest(IReadOnlyList<long> apprenticeshipIds) : IGetApiRequest
{
    private IReadOnlyList<long> ApprenticeshipIds { get; } = apprenticeshipIds;

    public string GetUrl => "api/employment-checks?" + string.Join("&", ApprenticeshipIds.Select(id => "apprenticeshipIds=" + id));
}
