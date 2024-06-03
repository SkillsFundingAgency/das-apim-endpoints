using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetCandidateAddressesByPostcode;
public class GetCandidateAddressesByPostcodeQueryResult
{
    public string? Postcode { get; set; }
    public string? Uprn { get; set; }
    public IEnumerable<GetAddressesListItem> Addresses { get; set; }

    public GetCandidateAddressesByPostcodeQueryResult(GetCandidateAddressApiResponse source, GetAddressesListResponse response, string postcode)
    {
        Postcode = postcode;
        Addresses = response.Addresses;
        Uprn = source?.Uprn;
    }
}
