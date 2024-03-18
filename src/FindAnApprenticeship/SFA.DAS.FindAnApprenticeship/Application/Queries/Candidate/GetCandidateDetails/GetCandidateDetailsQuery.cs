using MediatR;
using System;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Candidate.GetCandidateDetails;

public record GetCandidateDetailsQuery: IRequest<GetCandidateDetailsQueryResult>
{
    public required Guid CandidateId { get; set; }
}