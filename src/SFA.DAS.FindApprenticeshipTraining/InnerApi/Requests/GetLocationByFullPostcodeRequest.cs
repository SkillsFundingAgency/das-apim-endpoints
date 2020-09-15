using SFA.DAS.SharedOuterApi.Interfaces;
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
        public string GetUrl => $"api/postcodes?postcode={HttpUtility.UrlEncode(_fullPostcode)}";
    }
}
