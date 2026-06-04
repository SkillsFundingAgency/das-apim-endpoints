using MediatR;

namespace SFA.DAS.Aodp.Application.Commands.FormBuilder.Questions;

public class MoveQuestionDownCommand : IRequest<BaseMediatrResponse<MoveQuestionDownCommandResponse>>
{
    public Guid QuestionId { get; set; }
    public Guid PageId { get; set; }
    public Guid FormVersionId { get; set; }
    public Guid SectionId { get; set; }
}

