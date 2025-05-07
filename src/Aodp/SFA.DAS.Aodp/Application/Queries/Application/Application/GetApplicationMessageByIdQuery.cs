using MediatR;

namespace SFA.DAS.Aodp.Application.Queries.Application.Application;

public class GetApplicationMessageByIdQuery : IRequest<BaseMediatrResponse<GetApplicationMessageByIdQueryResponse>>
{
    public Guid MessageId { get; set; }
    public GetApplicationMessageByIdQuery(Guid messageId)
    {
        MessageId = messageId;
    }
}
