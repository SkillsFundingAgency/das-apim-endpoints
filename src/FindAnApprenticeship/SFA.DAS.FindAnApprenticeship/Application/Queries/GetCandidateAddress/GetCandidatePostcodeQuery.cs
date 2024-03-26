using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetCandidateAddress;
public class GetCandidatePostcodeQuery : IRequest<GetCandidatePostcodeQueryResult>
{
    public Guid CandidateId { get; set; }
}
