namespace SFA.DAS.EarlyConnect.ExternalModels
{
    public class ResponseAnswersDto
    {
        public int? Id { get; set; }
        public int QuestionId { get; set; }
        public int? AnswerId { get; set; }
        public string AnswerText { get; set; }
        public string Response { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
