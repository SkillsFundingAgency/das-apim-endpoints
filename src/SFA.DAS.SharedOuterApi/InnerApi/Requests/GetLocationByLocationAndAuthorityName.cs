using System.Web;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
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

        public string GetUrl  => $"api/locations?locationName={HttpUtility.UrlEncode(_locationName)}&authorityName={HttpUtility.UrlEncode(_authorityName)}";
    }
}