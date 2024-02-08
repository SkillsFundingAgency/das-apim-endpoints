using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EarlyConnect.InnerApi.Requests
{
    public class GetStudentTriageDataBySurveyIdRequest : IGetApiRequest
    {
        public Guid SurveyGuid { get; set; }
        public GetStudentTriageDataBySurveyIdRequest(Guid surveyGuid)
        {
            SurveyGuid = surveyGuid;
        }
        public string GetUrl => $"/api/student-triage-data/{SurveyGuid}";
    }
}