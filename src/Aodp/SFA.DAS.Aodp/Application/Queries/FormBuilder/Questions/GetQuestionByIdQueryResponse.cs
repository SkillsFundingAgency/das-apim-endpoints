public class GetQuestionByIdQueryResponse
{
    public Guid Id { get; set; }
    public Guid PageId { get; set; }
    public string Title { get; set; }
    public Guid Key { get; set; }
    public string Hint { get; set; }
    public string? Helper { get; set; }
    public string? HelperHTML { get; set; }
    public int Order { get; set; }
    public bool Required { get; set; }
    public string Type { get; set; }

    public TextInputOptions TextInput { get; set; } = new();
    public NumberInputOptions NumberInput { get; set; } = new();
    public CheckboxOptions Checkbox { get; set; } = new();
    public DateInputOptions DateInput { get; set; } = new();
    public FileUploadOptions FileUpload { get; set; } = new();

    public List<Option> Options { get; set; } = new();
    public List<RouteInformation> Routes { get; set; } = new();
    public bool Editable { get; set; }

    public class TextInputOptions
    {
        public int? MinLength { get; set; }
        public int? MaxLength { get; set; }
    }

    public class Option
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
        public int Order { get; set; }
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

    public class DateInputOptions
    {
        public DateOnly? GreaterThanOrEqualTo { get; set; }
        public DateOnly? LessThanOrEqualTo { get; set; }
        public bool? MustBeInFuture { get; set; }
        public bool? MustBeInPast { get; set; }
    }

    public class FileUploadOptions
    {
        public List<string> FileTypes { get; set; }
        public string? FileNamePrefix { get; set; }
        public int? NumberOfFiles { get; set; }
    }

    public class RouteInformation
    {
        public Page? NextPage { get; set; }
        public Section? NextSection { get; set; }
        public bool EndForm { get; set; }
        public bool EndSection { get; set; }
        public Option Option { get; set; }
    }

    public class Section
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int Order { get; set; }
        public List<Page> Pages { get; set; } = new();
    }

    public class Page
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int Order { get; set; }
    }
}