using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Qualifications
{
    public class GetNewQualificationCsvExportApiRequest : IGetApiRequest
    {
        public string GetUrl => "api/qualifications/export?status=new";
    }
}
