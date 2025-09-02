using MediatR;
using System;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetEmploymentLocations
{
    public record GetEmploymentLocationsQuery(Guid ApplicationId, Guid CandidateId)
        : IRequest<GetEmploymentLocationsQueryResult>;
}