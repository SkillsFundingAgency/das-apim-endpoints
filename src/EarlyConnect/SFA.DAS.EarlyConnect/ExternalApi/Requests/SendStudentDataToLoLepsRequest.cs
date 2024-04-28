using SFA.DAS.EarlyConnect.ExternalModels;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EarlyConnect.ExternalApi.Requests
{
    public class SendStudentDataToLoLepsRequest : IPostApiRequest
    {
        public Guid SurveyGuid { get; set; }
        public object Data { get; set; }

        public SendStudentDataToLoLepsRequest(StudentTriageData studentTriageData, Guid surveyGuid)
        {
            Data = studentTriageData;
            SurveyGuid = surveyGuid;
        }
        public string PostUrl => "triggers/manual/paths/invoke";
    }
}