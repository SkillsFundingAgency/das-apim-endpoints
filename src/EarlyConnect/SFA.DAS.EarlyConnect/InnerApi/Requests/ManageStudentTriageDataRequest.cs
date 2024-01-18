using SFA.DAS.EarlyConnect.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EarlyConnect.InnerApi.Requests
{
    public class ManageStudentTriageDataRequest : IPostApiRequest
    {
        public string SurveyGuid { get; set; }
        public object Data { get; set; }

        public ManageStudentTriageDataRequest(StudentTriageData studentTriageData, string surveyGuid)
        {
            Data = studentTriageData;
            SurveyGuid = surveyGuid;
        }
        public string PostUrl => $"/api/student-triage-data/{SurveyGuid}";
    }
}