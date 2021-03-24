using System.Web;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
{
    public class GetLocationByOutcodeAndDistrictRequest : IGetApiRequest
    {
        private readonly string _outcode;

        public GetLocationByOutcodeAndDistrictRequest(string outcode)
        {
            _outcode = outcode;
        }

        public string GetUrl => $"api/postcodes/outcode?outcode={HttpUtility.UrlEncode(_outcode)}";
    }
}