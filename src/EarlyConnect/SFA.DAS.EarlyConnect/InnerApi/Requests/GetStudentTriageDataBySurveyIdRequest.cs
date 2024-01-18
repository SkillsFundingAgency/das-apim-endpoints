using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EarlyConnect.InnerApi.Requests
{
    public class GetStudentTriageDataBySurveyIdRequest : IGetApiRequest
    {
        public string SurveyGuid { get; set; }
        public GetStudentTriageDataBySurveyIdRequest(string surveyGuid)
        {
            SurveyGuid = surveyGuid;
        }
        public string GetUrl => $"/api/student-triage-data/{SurveyGuid}";
    }
}