using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
{
    public class GetStandardsExportRequest : IGetApiRequest
    {
        public string GetUrl => "ops/export";
    }
}