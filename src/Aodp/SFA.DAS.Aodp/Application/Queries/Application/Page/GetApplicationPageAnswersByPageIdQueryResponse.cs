public class GetApplicationPageAnswersByPageIdQueryResponse 
{
    public List<Question> Questions { get; set; } = new();

    public class Question
    {
        public Guid QuestionId { get; set; }
        public Answer Answer { get; set; }
    }

    public class Answer
    {
        public string? TextValue { get; set; }
        public decimal? NumberValue { get; set; }
        public DateTime? DateValue { get; set; }
        public List<string>? MultipleChoiceValue { get; set; }
        public string? RadioChoiceValue { get; set; }
    }

}