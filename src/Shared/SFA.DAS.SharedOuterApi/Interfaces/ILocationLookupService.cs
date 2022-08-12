using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.SharedOuterApi.Interfaces
{
    public interface ILocationLookupService
    {
        Task<LocationItem> GetLocationInformation(string location, double lat,
            double lon, bool includeDistrictNameInPostcodeDisplayName = false);

        Task<GetAddressesListResponse> GetExactMatchAddresses(string fullPostcode);
    }
}