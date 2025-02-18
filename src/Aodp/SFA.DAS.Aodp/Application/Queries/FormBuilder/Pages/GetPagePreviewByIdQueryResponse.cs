
public class GetPagePreviewByIdQueryResponse
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Order { get; set; }

    public List<Question> Questions { get; set; }

    public class Question
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Type { get; set; }
        public bool Required { get; set; }
        public string? Hint { get; set; } = string.Empty;
        public int Order { get; set; }

        public List<Option> Options { get; set; } = new();

    }


    public class Option
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
        public int Order { get; set; }
    }
}