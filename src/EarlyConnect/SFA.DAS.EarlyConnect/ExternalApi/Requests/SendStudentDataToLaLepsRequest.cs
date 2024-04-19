using SFA.DAS.EarlyConnect.ExternalModels;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EarlyConnect.ExternalApi.Requests
{
    public class SendStudentDataToLaLepsRequest : IPostApiRequest
    {
        public Guid SurveyGuid { get; set; }
        public object Data { get; set; }

        public SendStudentDataToLaLepsRequest(StudentTriageData studentTriageData, Guid surveyGuid)
        {
            Data = studentTriageData;
            SurveyGuid = surveyGuid;
        }
        public string PostUrl => $"earlyConnect/triagedStudent/";
    }
}