namespace SFA.DAS.EarlyConnect.Models
{
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
}
