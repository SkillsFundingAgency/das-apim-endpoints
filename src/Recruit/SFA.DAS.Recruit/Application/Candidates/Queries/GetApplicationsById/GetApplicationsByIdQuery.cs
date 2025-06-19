using MediatR;
using System;
using System.Collections.Generic;

namespace SFA.DAS.Recruit.Application.Candidates.Queries.GetApplicationsById
{
    public sealed record GetApplicationsByIdQuery(List<Guid> ApplicationIds, bool IncludeDetails)
        : IRequest<GetApplicationsByIdQueryResult>;
}
