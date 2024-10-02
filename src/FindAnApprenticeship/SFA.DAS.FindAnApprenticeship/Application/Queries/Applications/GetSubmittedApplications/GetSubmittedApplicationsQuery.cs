using MediatR;
using System;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Applications.GetSubmittedApplications
{
    public record GetSubmittedApplicationsQuery(Guid CandidateId) : IRequest<GetSubmittedApplicationsQueryResult>;
}
