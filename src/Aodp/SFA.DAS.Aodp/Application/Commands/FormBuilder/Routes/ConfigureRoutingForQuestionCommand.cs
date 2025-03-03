using MediatR;
using SFA.DAS.Aodp.Application.Commands.FormBuilder.Questions;

namespace SFA.DAS.Aodp.Application.Commands.FormBuilder.Routes
{
    public class ConfigureRoutingForQuestionCommand : IRequest<BaseMediatrResponse<ConfigureRoutingForQuestionCommandResponse>>
    {
        public Guid FormVersionId { get; set; }
        public Guid SectionId { get; set; }
        public Guid PageId { get; set; }
        public Guid QuestionId { get; set; }

        public List<Route> Routes { get; set; }

        public class Route
        {
            public Guid OptionId { get; set; }
            public Guid? NextSectionId { get; set; }
            public Guid? NextPageId { get; set; }
            public bool EndForm { get; set; }
            public bool EndSection { get; set; }
        }
    }
}
