using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Qualifications
{
    public class GetChangedQualificationCsvExportApiRequest : IGetApiRequest
    {
        public string GetUrl => "api/qualifications/export?status=changed";
    }
}
