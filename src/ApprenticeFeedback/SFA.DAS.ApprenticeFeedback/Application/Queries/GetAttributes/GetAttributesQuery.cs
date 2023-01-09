using MediatR;

namespace SFA.DAS.ApprenticeFeedback.Application.Queries.GetAttributes
{
    public class GetAttributesQuery : IRequest<GetAttributesResult>
    {
        public string AttributeType { get; set; }
    }
}
