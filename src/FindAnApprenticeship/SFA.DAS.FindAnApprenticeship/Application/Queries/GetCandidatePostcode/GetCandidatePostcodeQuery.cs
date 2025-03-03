using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetCandidatePostcode;

public class GetCandidatePostcodeQuery : IRequest<GetCandidatePostcodeQueryResult>
{
    public Guid CandidateId { get; set; }
}