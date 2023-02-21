using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.ApprenticeAan.Application.Locations.Queries.GetAddresses
{
    public class GetAddressesQueryResult
    {
        public GetAddressesQueryResult(GetAddressesListResponse response) => AddressesResponse = response;

        public GetAddressesListResponse AddressesResponse { get; }
    }
}