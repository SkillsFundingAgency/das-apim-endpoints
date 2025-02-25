
namespace SFA.DAS.Aodp.Application.Queries.FormBuilder.Questions;

public class GetQuestionByIdQueryResponse
{
    public Guid Id { get; set; }
    public Guid PageId { get; set; }
    public string Title { get; set; }
    public Guid Key { get; set; }
    public string Hint { get; set; }
    public int Order { get; set; }
    public bool Required { get; set; }
    public string Type { get; set; }

    public TextInputOptions TextInput { get; set; } = new();
    public List<RadioOptionItem> RadioOptions { get; set; } = new();
    public bool Editable { get; set; }

    public class TextInputOptions
    {
        public int? MinLength { get; set; }
        public int? MaxLength { get; set; }
    }

    public class RadioOptionItem
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
        public int Order { get; set; }
    }

}