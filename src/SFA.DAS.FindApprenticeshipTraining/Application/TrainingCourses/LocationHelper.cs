using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SFA.DAS.FindApprenticeshipTraining.Configuration;
using SFA.DAS.FindApprenticeshipTraining.Domain.Models;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipTraining.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses
{
    internal class LocationHelper
    {
        private readonly ILocationApiClient<LocationApiConfiguration> _locationApiClient;
        private const string PostcodeRegex = @"^[A-Za-z]{1,2}\d[A-Za-z\d]?\s*\d[A-Za-z]{2}$";
        
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
            if (location.Split(",").Length == 2)
            {
                
                var locationInformation = location.Split(",");
                var locationName = locationInformation.First().Trim();
                var authorityName = locationInformation.Last().Trim();
                getLocationsListItem = await _locationApiClient.Get<GetLocationsListItem>(new GetLocationByLocationAndAuthorityName(locationName, authorityName));
            }   
            if (Regex.IsMatch(location, PostcodeRegex))
            {
                getLocationsListItem =  await _locationApiClient.Get<GetLocationsListItem>(new GetLocationByFullPostcodeRequest(location));
            }

            return getLocationsListItem != null 
                ? new LocationItem(location, getLocationsListItem.Location.GeoPoint) 
                : null;
        }
    }
}