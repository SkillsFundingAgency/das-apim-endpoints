using MediatR;

namespace SFA.DAS.Aodp.Application.Queries.Application.Application;

public class GetApplicationMessagesByIdQuery : IRequest<BaseMediatrResponse<GetApplicationMessagesByIdQueryResponse>>
{
    public Guid ApplicationId { get; set; }
    public GetApplicationMessagesByIdQuery(Guid applicationId)
    {
        ApplicationId = applicationId;
    }
}