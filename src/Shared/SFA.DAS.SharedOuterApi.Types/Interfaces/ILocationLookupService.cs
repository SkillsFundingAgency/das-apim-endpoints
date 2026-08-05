using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Location;
using SFA.DAS.SharedOuterApi.Types.Models;

namespace SFA.DAS.SharedOuterApi.Types.Interfaces;

public interface ILocationLookupService
{
    Task<LocationItem> GetLocationInformation(string location, double lat,
        double lon, bool includeDistrictNameInPostcodeDisplayName = false);

    Task<GetAddressesListResponse> GetExactMatchAddresses(string fullPostcode);
    Task<PostcodeInfo?> GetPostcodeInfoAsync(string postcode);
}