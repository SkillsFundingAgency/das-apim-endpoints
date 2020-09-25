using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SFA.DAS.FindApprenticeshipTraining.Configuration;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipTraining.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses
{
    internal class LocationHelper
    {
        private readonly ILocationApiClient<LocationApiConfiguration> _locationApiClient;

        public LocationHelper (ILocationApiClient<LocationApiConfiguration> locationApiClient)
        {
            _locationApiClient = locationApiClient;
        }

        public async Task<GetLocationsListItem> GetLocationInformation(string location)
        {
            string postcodeRegex = @"^[A-Za-z]{1,2}\d[A-Za-z\d]?\s*\d[A-Za-z]{2}$";

            if (string.IsNullOrEmpty(location))
            {
                return null;
            }

            if (location.Split(",").Length == 2)
            {
                var locationInformation = location.Split(",");
                var locationName = locationInformation.First().Trim();
                var authorityName = locationInformation.Last().Trim();
                return await _locationApiClient.Get<GetLocationsListItem>(new GetLocationByLocationAndAuthorityName(locationName, authorityName));
            }   
            if (Regex.IsMatch(location, postcodeRegex))
            {
                return await _locationApiClient.Get<GetLocationsListItem>(new GetLocationByFullPostcodeRequest(location));
            }

            if (!string.IsNullOrEmpty(location))
            {
                var locationName = location.Trim();
                return await _locationApiClient.Get<GetLocationsListItem>(new GetLocationByLocationName(locationName));
            }

            return null;
        }
    }
}