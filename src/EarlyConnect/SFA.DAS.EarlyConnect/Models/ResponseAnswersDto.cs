namespace SFA.DAS.EarlyConnect.Models
{
    public class ResponseAnswersDto
    {
        public int? Id { get; set; }
        public Guid StudentSurveyId { get; set; }
        public int QuestionId { get; set; }
        public int? AnswerId { get; set; }
        public string Response { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
