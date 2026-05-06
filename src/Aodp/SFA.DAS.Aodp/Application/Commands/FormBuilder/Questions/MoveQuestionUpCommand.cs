using MediatR;

namespace SFA.DAS.Aodp.Application.Commands.FormBuilder.Questions;

public class MoveQuestionUpCommand : IRequest<BaseMediatrResponse<MoveQuestionUpCommandResponse>>
{
    public Guid QuestionId { get; set; }
    public Guid PageId { get; set; }
    public Guid FormVersionId { get; set; }
    public Guid SectionId { get; set; }
}
