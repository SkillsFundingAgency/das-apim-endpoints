using MediatR;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using System;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Applications.GetApplicationsCount
{
    public record GetApplicationsCountQuery(Guid CandidateId, ApplicationStatus Status)
        : IRequest<GetApplicationsCountQueryResult>;
}