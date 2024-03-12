namespace SFA.DAS.EarlyConnect.Api.Models
{
    public class CreateStudentFeedbackPostRequest
    {
        public IEnumerable<StudentFeedbackRequestModel> ListOfStudentFeedback { get; set; }
    }
}
