using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetCandidatePostcodeAddress;
public class GetCandidatePostcodeAddressQuery : IRequest<GetCandidatePostcodeAddressQueryResult>
{
    public string Postcode { get; set; }
}
