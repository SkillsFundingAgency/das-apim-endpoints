using System.Web;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests
{
    public class GetLocationByLocationAndAuthorityName : IGetApiRequest
    {
        private readonly string _locationName;
        private readonly string _authorityName;

        public GetLocationByLocationAndAuthorityName(string locationName, string authorityName)
        {
            _locationName = locationName;
            _authorityName = authorityName;
        }

        public string BaseUrl { get; set; }
        public string GetUrl  => $"{BaseUrl}api/locations?locationName={HttpUtility.UrlEncode(_locationName)}&authorityName={HttpUtility.UrlEncode(_authorityName)}";
    }
}