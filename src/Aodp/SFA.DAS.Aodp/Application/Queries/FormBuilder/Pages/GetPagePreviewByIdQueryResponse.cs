namespace SFA.DAS.Aodp.Application.Queries.FormBuilder.Pages;

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

        public TextInputOptions TextInput { get; set; } = new();
        public RadioOptions RadioButton { get; set; } = new();

    }

    public class TextInputOptions
    {
        public int? MinLength { get; set; }
        public int? MaxLength { get; set; }

    }

    public class RadioOptions
    {
        public List<RadioOptionItem> MultiChoice { get; set; } = new();

        public class RadioOptionItem
        {
            public Guid Id { get; set; }
            public string Value { get; set; }
            public int Order { get; set; }
        }
    }
}