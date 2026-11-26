using System.Collections.Generic;
using MediatR;

namespace SFA.DAS.Approvals.Application.EmploymentChecks.Queries.GetEmploymentChecksQuery;

public class GetEmploymentChecksQuery : IRequest<GetEmploymentChecksResult>
{
    public IReadOnlyList<long> ApprenticeshipIds { get; set; }
}
