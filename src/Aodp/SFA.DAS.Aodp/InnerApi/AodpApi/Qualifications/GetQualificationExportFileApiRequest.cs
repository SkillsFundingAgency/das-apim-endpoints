using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Qualifications
{
    public class GetQualificationExportFileApiRequest : IGetApiRequest
    {
        public string GetUrl => "api/qualifications/export";
    }
}
