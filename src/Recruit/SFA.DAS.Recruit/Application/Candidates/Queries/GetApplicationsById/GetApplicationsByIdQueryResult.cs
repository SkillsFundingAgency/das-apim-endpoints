using System.Collections.Generic;

namespace SFA.DAS.Recruit.Application.Candidates.Queries.GetApplicationsById
{
    public sealed record GetApplicationsByIdQueryResult(List<Domain.Application> Applications);
}
