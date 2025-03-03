using MediatR;

namespace SFA.DAS.Aodp.Application.Queries.FormBuilder.Questions;

public class GetQuestionByIdQuery : IRequest<BaseMediatrResponse<GetQuestionByIdQueryResponse>>
{
    public Guid QuestionId { get; set; }
    public Guid SectionId { get; set; }
    public Guid FormVersionId { get; set; }
    public Guid PageId { get; set; }
}