using System;
using System.Web;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
{
    [Obsolete("GetLocationByFullPostcodeRequestV2 should be used instead, note it returns a slightly different response to this request")]
    public class GetLocationByFullPostcodeRequest : IGetApiRequest
    {
        private readonly string _fullPostcode;

        public GetLocationByFullPostcodeRequest(string fullPostcode)
        {
            _fullPostcode = fullPostcode;            
        }
        public string GetUrl => $"api/postcodes?postcode={HttpUtility.UrlEncode(_fullPostcode)}";
    }
}
