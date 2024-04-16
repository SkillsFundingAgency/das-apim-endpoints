using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetCandidateAddressesByPostcode;
public class GetCandidateAddressesByPostcodeQuery : IRequest<GetCandidateAddressesByPostcodeQueryResult>
{
    public Guid CandidateId { get; set; }
    public string? Postcode { get; set; }
}
