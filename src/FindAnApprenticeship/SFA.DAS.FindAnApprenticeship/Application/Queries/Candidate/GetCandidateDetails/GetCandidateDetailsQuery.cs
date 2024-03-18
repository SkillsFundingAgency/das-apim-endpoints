using System;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Candidate.GetCandidateDetails;

public record GetCandidateDetailsQuery
{
    public required Guid CandidateId { get; set; }
}