using MediatR;
using SFA.DAS.Aodp.Validation;

namespace SFA.DAS.Aodp.Application.Commands.FormBuilder.Questions;

public class CreateQuestionCommand : IRequest<BaseMediatrResponse<CreateQuestionCommandResponse>>
{
    public Guid FormVersionId { get; set; }
    public Guid SectionId { get; set; }
    public Guid PageId { get; set; }

    [AllowedCharacters(TextCharacterProfile.Title)]
    public string Title { get; set; }
    public bool Required { get; set; }

    [QuestionType]
    public string Type { get; set; }
}
