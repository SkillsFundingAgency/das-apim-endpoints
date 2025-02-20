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
        public NumberInputOptions NumberInput { get; set; } = new();
        public CheckboxOptions Checkbox { get; set; } = new();
        public List<Option> Options { get; set; } = new();
        public DateInputOptions DateInput { get; set; } = new();
        public FileUploadOptions FileUpload { get; set; } = new();

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

    public class CheckboxOptions
    {
        public int? MinNumberOfOptions { get; set; }
        public int? MaxNumberOfOptions { get; set; }
    }

    public class NumberInputOptions
    {
        public int? GreaterThanOrEqualTo { get; set; }
        public int? LessThanOrEqualTo { get; set; }
        public int? NotEqualTo { get; set; }
    }


    public class Option
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
        public int Order { get; set; }
    }

    public class DateInputOptions
    {
        public DateOnly? GreaterThanOrEqualTo { get; set; }
        public DateOnly? LessThanOrEqualTo { get; set; }
        public bool? MustBeInFuture { get; set; }
        public bool? MustBeInPast { get; set; }
    }

    public class FileUploadOptions
    {
        public int? MaxSize { get; set; }
        public string? FileNamePrefix { get; set; }
        public int? NumberOfFiles { get; set; }
    }

}
