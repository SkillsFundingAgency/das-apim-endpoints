using System.Web;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
{
    public class GetLocationByOutcodeRequest : IGetApiRequest
    {
        private readonly string _outcode;

        public GetLocationByOutcodeRequest(string outcode)
        {
            _outcode = outcode;

        }

        public string GetUrl => $"api/search?query={HttpUtility.UrlEncode(_outcode)}";

    }
}
