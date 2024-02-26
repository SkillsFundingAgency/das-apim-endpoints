namespace SFA.DAS.EarlyConnect.Application.Commands.CreateOtherStudentTriageData
{
    public class CreateOtherStudentTriageDataCommandResult
    {
        public string StudentSurveyId { get; set; }
        public string AuthCode { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
