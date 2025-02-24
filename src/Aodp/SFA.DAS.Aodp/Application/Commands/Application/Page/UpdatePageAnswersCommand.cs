using MediatR;

public class UpdatePageAnswersCommand : IRequest<BaseMediatrResponse<UpdatePageAnswersCommandResponse>>
{
    public Guid PageId { get; set; }
    public Guid FormVersionId { get; set; }
    public Guid SectionId { get; set; }
    public List<Question> Questions { get; set; } = new();
    public Guid ApplicationId { get; set; }

    public Route Routing { get; set; } = new();


    public class Route
    {
        public Guid QuestionId { get; set; }
        public Guid OptionId { get; set; }
        public int? NextPageOrder { get; set; }
        public int? NextSectionOrder { get; set; }
        public bool EndForm { get; set; }
        public bool EndSection { get; set; }
    }

    public class Question
    {
        public Guid QuestionId { get; set; }
        public string QuestionType { get; set; }
        public Answer Answer { get; set; }
    }

    public class Answer
    {
        public string? TextValue { get; set; }
        public decimal? NumberValue { get; set; }
        public DateTime? DateValue { get; set; }
        public List<string>? MultipleChoiceValue { get; set; }
        public string RadioChoiceValue { get; set; }
    }

}