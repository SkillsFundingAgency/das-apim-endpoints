using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests
{
    public class GetLocationByLocationName : IGetApiRequest
    {
        private readonly string _locationName;

        public GetLocationByLocationName(string locationName)
        {
            _locationName = locationName;
        }

        public string GetUrl => $"api/locations?locationName={HttpUtility.UrlEncode(_locationName)}";

    }
}
