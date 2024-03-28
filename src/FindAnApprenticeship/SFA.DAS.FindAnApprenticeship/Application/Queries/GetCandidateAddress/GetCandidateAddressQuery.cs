using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetCandidateAddress;
public class GetCandidateAddressQuery : IRequest<GetCandidateAddressQueryResult>
{
    public Guid CandidateId { get; set; }
}
