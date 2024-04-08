using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetCandidateName;
public class GetCandidateNameQuery : IRequest<GetCandidateNameQueryResult>
{
    public Guid CandidateId { get; set; }
}
