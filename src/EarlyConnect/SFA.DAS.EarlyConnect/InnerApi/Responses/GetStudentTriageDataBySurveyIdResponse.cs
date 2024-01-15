namespace SFA.DAS.EarlyConnect.InnerApi.Responses
{
    public class GetStudentTriageDataBySurveyIdResponse
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
        public ICollection<SurveyQuestionsDto> SurveyQuestions { get; set; }
        public StudentSurveyDto StudentSurvey { get; set; }
    }

    public class SurveyQuestionsDto
    {
        public int Id { get; set; }
        public int SurveyId { get; set; }
        public int QuestionTypeId { get; set; }
        public string QuestionText { get; set; }
        public string ShortDescription { get; set; }
        public string SummaryLabel { get; set; }
        public string ValidationMessage { get; set; }
        public int? DefaultToggleAnswerId { get; set; }
        public int SortOrder { get; set; }
        public ICollection<AnswersDto> Answers { get; set; }
    }
    public class AnswersDto
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string AnswerText { get; set; }
        public string ShortDescription { get; set; }
        public int GroupNumber { get; set; }
        public int GroupLabel { get; set; }
        public int SortOrder { get; set; }
    }
    public class StudentSurveyDto
    {
        public Guid Id { get; set; }
        public int StudentId { get; set; }
        public int SurveyId { get; set; }
        public DateTime? LastUpdated { get; set; }
        public DateTime? DateCompleted { get; set; }
        public DateTime? DateEmailSent { get; set; }
        public DateTime DateAdded { get; set; }
        public ICollection<ResponseAnswersDto> ResponseAnswers { get; set; }
    }
    public class ResponseAnswersDto
    {
        public int Id { get; set; }
        public Guid StudentSurveyId { get; set; }
        public int QuestionId { get; set; }
        public int? AnswerId { get; set; }
        public string Response { get; set; }
        public DateTime DateAdded { get; set; }
    }
}

