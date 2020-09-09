﻿using System.Linq;
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
            if (!string.IsNullOrEmpty(location) && location.Split(",").Length == 2)
            {
                var locationInformation = location.Split(",");
                var locationName = locationInformation.First().Trim();
                var authorityName = locationInformation.Last().Trim();
                return await _locationApiClient.Get<GetLocationsListItem>(new GetLocationByLocationAndAuthorityName(locationName, authorityName));
            }   
            
            return null;
        }
    }
}