namespace SFA.DAS.EarlyConnect.Api.Models
{
    public class ManageStudentTriageDataPostRequest
    {
        public int Id { get; set; }
        public int? LepsId { get; set; }
        public int? LogId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string Postcode { get; set; }
        public string DataSource { get; set; }
        public string Industry { get; set; }
        public DateTime? DateInterest { get; set; }
        public StudentSurveyRequest StudentSurvey { get; set; }
    }
    public class StudentSurveyRequest
    {
        public Guid Id { get; set; }
        public int StudentId { get; set; }
        public int SurveyId { get; set; }
        public DateTime? LastUpdated { get; set; }
        public DateTime? DateCompleted { get; set; }
        public DateTime? DateEmailSent { get; set; }
        public DateTime DateAdded { get; set; }
        public ICollection<AnswersRequest> ResponseAnswers { get; set; }
    }
    public class AnswersRequest
    {
        public int? Id { get; set; }
        public Guid StudentSurveyId { get; set; }
        public int QuestionId { get; set; }
        public int? AnswerId { get; set; }
        public string Response { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
