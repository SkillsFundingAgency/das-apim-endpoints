using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Qualifications
{
    public class GetQualificationOutputFileApiRequest : IGetApiRequest
    {
        public string GetUrl => "api/qualifications/outputfile";
    }
}
