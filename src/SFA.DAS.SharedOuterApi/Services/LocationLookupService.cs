using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.SharedOuterApi.Services
{
    public class LocationLookupService : ILocationLookupService
    {
        private readonly ILocationApiClient<LocationApiConfiguration> _locationApiClient;
        private const string PostcodeRegex = @"^[A-Za-z]{1,2}\d[A-Za-z\d]?\s*\d[A-Za-z]{2}$";
        private const string OutcodeRegex = @"^[A-Za-z]{1,2}\d[A-Za-z\d]?";
        private const string OutcodeDistrictRegex = @"^[A-Za-z]{1,2}\d[A-Za-z\d]?\s[A-Za-z]*";

        public LocationLookupService(ILocationApiClient<LocationApiConfiguration> locationApiClient)
        {
            _locationApiClient = locationApiClient;
        }

        public async Task<LocationItem> GetLocationInformation(string location, double lat, double lon, bool includeDistrictNameInPostcodeDisplayName = false)
        {
            if (string.IsNullOrEmpty(location))
            {
                return null;
            }
            
            if (lat != 0 && lon != 0)
            {
                return new LocationItem(location, new []{ lat, lon}, string.Empty);
            }

            GetLocationsListItem getLocationsListItem  = null;
            
            if (Regex.IsMatch(location, PostcodeRegex))
            {
                getLocationsListItem =  await _locationApiClient.Get<GetLocationsListItem>(new GetLocationByFullPostcodeRequest(location));
                getLocationsListItem.IncludeDistrictNameInPostcodeDisplayName = includeDistrictNameInPostcodeDisplayName;
                location = getLocationsListItem.DisplayName;
            }
            else if (Regex.IsMatch(location, OutcodeDistrictRegex))
            {
                getLocationsListItem = await _locationApiClient.Get<GetLocationsListItem>(new GetLocationByOutcodeAndDistrictRequest(location.Split(' ').FirstOrDefault()));
                if (getLocationsListItem.Location != null)
                {
                    location = getLocationsListItem.DisplayName;
                }
            }
            else if(Regex.IsMatch(location, OutcodeRegex))
            {
                getLocationsListItem = await _locationApiClient.Get<GetLocationsListItem>(new GetLocationByOutcodeRequest(location));
            }
            else if (location.Split(",").Length >= 2)
            {
                
                var locationInformation = location.Split(",");
                var locationName = string.Join(",",locationInformation.Take(locationInformation.Length-1)).Trim();
                var authorityName = locationInformation.Last().Trim();
                getLocationsListItem = await _locationApiClient.Get<GetLocationsListItem>(new GetLocationByLocationAndAuthorityName(locationName, authorityName));
            }
            
            if (location.Length >= 2 && getLocationsListItem?.Location == null)
            {
                var locations = await _locationApiClient.Get<GetLocationsListResponse>(new GetLocationsQueryRequest(location));

                var locationsListItem = locations?.Locations?.FirstOrDefault();
                if (locationsListItem != null)
                {
                    getLocationsListItem = locationsListItem;
                    location = getLocationsListItem.DisplayName;    
                }
                
            }

            return getLocationsListItem?.Location != null
                ? new LocationItem(location, getLocationsListItem.Location.GeoPoint, getLocationsListItem.Country) 
                : null;
        }
    }
}