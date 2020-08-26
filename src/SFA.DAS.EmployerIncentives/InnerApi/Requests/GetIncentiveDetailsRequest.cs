using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests
{
    public class GetIncentiveDetailsRequest : IGetApiRequest
    {
        public GetIncentiveDetailsRequest()
        {
        }

        public string BaseUrl { get; set; }

        public string GetUrl => $"{BaseUrl}newapprenticeincentive";
    }
}