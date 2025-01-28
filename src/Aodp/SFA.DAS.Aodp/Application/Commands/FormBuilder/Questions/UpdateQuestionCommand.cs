using MediatR;

namespace SFA.DAS.Aodp.Application.Commands.FormBuilder.Questions;

public class UpdateQuestionCommand : IRequest<BaseMediatrResponse<UpdateQuestionCommandResponse>>
{
    public Guid Id { get; set; }
    public Guid FormVersionId { get; set; }
    public Guid SectionId { get; set; }
    public Guid PageId { get; set; }
    public string Title { get; set; }
    public string Hint { get; set; }
    public bool Required { get; set; }
    public TextInputOptions TextInput { get; set; } = new();
    public List<RadioOptionItem> RadioOptions { get; set; } = new();

    public class TextInputOptions
    {
        public int? MinLength { get; set; }
        public int? MaxLength { get; set; }
    }

    public class RadioOptionItem
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
    }
}