namespace SFA.DAS.EarlyConnect.InnerApi.Responses
{
    public class CreateOtherStudentTriageDataResponse
    {
        public string StudentSurveyId { get; set; }
        public string AuthCode { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}