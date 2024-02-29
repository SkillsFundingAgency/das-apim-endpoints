using SFA.DAS.EarlyConnect.ExternalModels;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EarlyConnect.ExternalApi.Requests
{
    public class SendStudentDataToNeLepsRequest : IPostApiRequest
    {
        public Guid SurveyGuid { get; set; }
        public object Data { get; set; }

        public SendStudentDataToNeLepsRequest(StudentTriageData studentTriageData, Guid surveyGuid)
        {
            Data = studentTriageData;
            SurveyGuid = surveyGuid;
        }
        public string PostUrl => $"leps/student/";
    }
}