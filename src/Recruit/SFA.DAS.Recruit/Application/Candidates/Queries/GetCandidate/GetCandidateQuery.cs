using System;
using MediatR;

namespace SFA.DAS.Recruit.Application.Candidates.Queries.GetCandidate;

public class GetCandidateQuery : IRequest<GetCandidateQueryResult>
{
    public Guid CandidateId { get; set; }
}