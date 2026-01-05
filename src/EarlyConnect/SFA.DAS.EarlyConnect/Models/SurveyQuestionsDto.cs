namespace SFA.DAS.EarlyConnect.Models
{
    public class SurveyQuestionsDto
    {
        public int Id { get; set; }
        public int SurveyId { get; set; }
        public int QuestionTypeId { get; set; }
        public string QuestionText { get; set; }
        public string ShortDescription { get; set; }
        public string SummaryLabel { get; set; }
        public string GroupLabel { get; set; }
        public int GroupNumber { get; set; }
        public string ValidationMessage { get; set; }
        public int? DefaultToggleAnswerId { get; set; }
        public int SortOrder { get; set; }
        public ICollection<AnswersDto> Answers { get; set; }
    }
}
