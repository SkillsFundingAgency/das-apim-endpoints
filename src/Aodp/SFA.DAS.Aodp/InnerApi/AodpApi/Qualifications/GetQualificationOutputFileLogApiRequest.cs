using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Qualifications
{
    public class GetQualificationOutputFileLogApiRequest : IGetApiRequest
    {
        public string GetUrl => $"api/qualifications/outputfile/logs";
    }
}
