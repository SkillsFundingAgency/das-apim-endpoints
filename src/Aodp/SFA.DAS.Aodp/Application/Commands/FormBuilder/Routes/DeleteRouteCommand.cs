using MediatR;

namespace SFA.DAS.Aodp.Application.Commands.FormBuilder.Routes
{
    public class DeleteRouteCommand : IRequest<BaseMediatrResponse<EmptyResponse>>
    {
        public Guid FormVersionId { get; set; }
        public Guid SectionId { get; set; }
        public Guid PageId { get; set; }
        public Guid QuestionId { get; set; }
    }
}
