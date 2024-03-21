namespace SFA.DAS.EarlyConnect.ExternalModels
{
    public class SurveyQuestionsDto
    {
        public int QuestionId { get; set; }
        public string QuestionType { get; set; }
        public string QuestionText { get; set; }
        public string ShortDescription { get; set; }
        public string SummaryLabel { get; set; }
        public int SortOrder { get; set; }
        public ICollection<AnswersDto> Answers { get; set; }
        public ICollection<ResponseAnswersDto> StudentResponse { get; set; }
    }
}
