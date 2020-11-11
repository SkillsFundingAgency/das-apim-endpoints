using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests
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
