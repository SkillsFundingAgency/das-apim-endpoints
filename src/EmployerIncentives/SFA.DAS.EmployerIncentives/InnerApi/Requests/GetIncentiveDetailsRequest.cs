using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests
{
    public class GetIncentiveDetailsRequest : IGetApiRequest
    {
        public string GetUrl => "newapprenticeincentive";
    }
}