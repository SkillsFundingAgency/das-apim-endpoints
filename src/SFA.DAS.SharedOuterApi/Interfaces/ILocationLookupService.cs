using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.SharedOuterApi.Interfaces
{
    public interface ILocationLookupService
    {
        Task<LocationItem> GetLocationInformation(string location, double lat,
            double lon, bool includeDistrictNameInPostcodeDisplayName = false);
    }
}