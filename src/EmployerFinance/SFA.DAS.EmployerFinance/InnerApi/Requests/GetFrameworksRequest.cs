using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.EmployerFinance.InnerApi.Requests
{
    public class GetFrameworksRequest : IGetApiRequest
    {
        public string GetUrl => "api/courses/frameworks";
    }
}