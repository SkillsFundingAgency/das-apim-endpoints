using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Qualifications
{
    public class GetQualificationOutputFileApiRequest : IGetApiRequest
    {
        public string CurrentUsername { get; set; } = string.Empty;

        public string GetUrl => "api/qualifications/output-file";

        public GetQualificationOutputFileApiRequest(string username) 
        {
            CurrentUsername = username;
        }
    }
}
