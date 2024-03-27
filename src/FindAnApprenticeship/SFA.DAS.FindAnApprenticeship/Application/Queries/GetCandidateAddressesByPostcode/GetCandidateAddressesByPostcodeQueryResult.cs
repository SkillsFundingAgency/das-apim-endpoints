using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetCandidateAddressesByPostcode;
public class GetCandidateAddressesByPostcodeQueryResult
{
    public GetAddressesListResponse AddressesResponse { get; }

    public GetCandidateAddressesByPostcodeQueryResult(GetAddressesListResponse response)
    {
        AddressesResponse = response;
    }
}
