using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests
{
    public class GetLocationByFullPostcodeRequest : IGetApiRequest
    {
        private readonly string _fullPostcode;

        public GetLocationByFullPostcodeRequest(string fullPostcode)
        {
            _fullPostcode = fullPostcode;            
        }
        public string BaseUrl { get; set; }
        public string GetUrl => $"{BaseUrl}api/postcodes?postcode={HttpUtility.UrlEncode(_fullPostcode)}";
    }
}
