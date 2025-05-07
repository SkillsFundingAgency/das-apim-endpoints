using MediatR;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using System;
using System.Collections.Generic;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Applications.GetApplicationsCount
{
    public record GetApplicationsCountQuery(Guid CandidateId, List<ApplicationStatus> Statuses)
        : IRequest<GetApplicationsCountQueryResult>;
}