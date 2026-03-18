using System.Web;

using SFA.DAS.Apim.Shared.Interfaces;

using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Location
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
