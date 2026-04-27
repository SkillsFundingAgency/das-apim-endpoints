using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests
{
    public class GetIncentiveDetailsRequest : IGetApiRequest
    {
        public string GetUrl => "newapprenticeincentive";
    }
}