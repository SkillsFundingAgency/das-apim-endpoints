using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetCandidateAddressesByPostcode;
public class GetCandidateAddressesByPostcodeQuery : IRequest<GetCandidateAddressesByPostcodeQueryResult>
{
    public GetCandidateAddressesByPostcodeQuery(string postcode)
    {
        Postcode = postcode;
    }

    public string Postcode { get; set; }
}
