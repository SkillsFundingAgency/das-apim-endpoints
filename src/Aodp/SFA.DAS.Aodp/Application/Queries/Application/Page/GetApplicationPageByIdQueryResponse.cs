public class GetApplicationPageByIdQueryResponse
{

    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Order { get; set; }
    public int TotalSectionPages { get; set; }

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
        public List<RouteInformation> Routes { get; set; } = new();

    }

    public class RouteInformation
    {
        public Guid OptionId { get; set; }
        public int? NextPageOrder { get; set; }
        public int? NextSectionOrder { get; set; }
        public bool EndForm { get; set; }
        public bool EndSection { get; set; }
    }

    public class Answer
    {
        public string? TextValue { get; set; }
        public int? IntegerValue { get; set; }
        public DateTime? DateValue { get; set; }
        public string? MultipleChoiceValue { get; set; }
        public bool? BooleanValue { get; set; }
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
