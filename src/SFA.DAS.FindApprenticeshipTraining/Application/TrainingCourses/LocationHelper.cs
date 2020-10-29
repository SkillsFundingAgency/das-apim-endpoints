using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SFA.DAS.FindApprenticeshipTraining.Domain.Models;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses
{
    internal class LocationHelper
    {
        private readonly ILocationApiClient<LocationApiConfiguration> _locationApiClient;
        private const string PostcodeRegex = @"^[A-Za-z]{1,2}\d[A-Za-z\d]?\s*\d[A-Za-z]{2}$";
        private const string OutcodeRegex = @"^[A-Za-z]{1,2}\d[A-Za-z\d]?";
        private const string OutcodeDistrictRegex = @"^[A-Za-z]{1,2}\d[A-Za-z\d]?\s[A-Za-z]*";

        public LocationHelper (ILocationApiClient<LocationApiConfiguration> locationApiClient)
        {
            _locationApiClient = locationApiClient;
        }

        public async Task<LocationItem> GetLocationInformation(string location)
        {
            if (string.IsNullOrEmpty(location))
            {
                return null;
            }

            GetLocationsListItem getLocationsListItem  = null;
            
            if (Regex.IsMatch(location, PostcodeRegex))
            {
                getLocationsListItem =  await _locationApiClient.Get<GetLocationsListItem>(new GetLocationByFullPostcodeRequest(location));
            }
            else if (Regex.IsMatch(location, OutcodeDistrictRegex))
            {
                getLocationsListItem = await _locationApiClient.Get<GetLocationsListItem>(new GetLocationByOutcodeAndDistrictRequest(location.Split(' ').FirstOrDefault()));
            }
            else if(Regex.IsMatch(location, OutcodeRegex))
            {
                getLocationsListItem = await _locationApiClient.Get<GetLocationsListItem>(new GetLocationByOutcodeRequest(location));
            }
            else if (location.Split(",").Length == 2)
            {
                
                var locationInformation = location.Split(",");
                var locationName = locationInformation.First().Trim();
                var authorityName = locationInformation.Last().Trim();
                getLocationsListItem = await _locationApiClient.Get<GetLocationsListItem>(new GetLocationByLocationAndAuthorityName(locationName, authorityName));
            }

            return getLocationsListItem?.Location != null
                ? new LocationItem(location, getLocationsListItem.Location.GeoPoint) 
                : null;
        }
    }
}