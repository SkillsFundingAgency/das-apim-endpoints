namespace SFA.DAS.EarlyConnect.Models
{
    public class AnswersDto
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string AnswerText { get; set; }
        public string ShortDescription { get; set; }
        public int GroupNumber { get; set; }
        public string GroupLabel { get; set; }
        public int SortOrder { get; set; }
    }
}
