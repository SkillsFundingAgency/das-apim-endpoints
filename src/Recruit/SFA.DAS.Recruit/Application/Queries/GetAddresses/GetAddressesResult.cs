using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Location;

namespace SFA.DAS.Recruit.Application.Queries.GetAddresses
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
