using SFA.DAS.Apim.Shared.Interfaces;

using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses
{
    public class GetStandardsExportRequest : IGetApiRequest
    {
        public string GetUrl => "ops/export";
    }
}