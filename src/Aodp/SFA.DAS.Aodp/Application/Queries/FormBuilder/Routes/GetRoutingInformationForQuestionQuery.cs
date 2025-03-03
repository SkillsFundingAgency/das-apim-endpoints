using MediatR;

namespace SFA.DAS.Aodp.Application.Queries.FormBuilder.Routes
{
    public class GetRoutingInformationForQuestionQuery : IRequest<BaseMediatrResponse<GetRoutingInformationForQuestionQueryResponse>>
    {
        public Guid FormVersionId { get; set; }
        public Guid QuestionId { get; set; }
        public Guid PageId { get;  set; }
        public Guid SectionId { get;  set; }
    }
}