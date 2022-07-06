using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetAddresses
{
    public class GetAddressesQueryResult
    {
        public GetAddressesListResponse AddressesResponse { get; }

        public GetAddressesQueryResult(GetAddressesListResponse response)
        {
            AddressesResponse = response;
        }
    }
}
