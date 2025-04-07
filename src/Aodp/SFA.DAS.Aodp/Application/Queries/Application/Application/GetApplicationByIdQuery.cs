using MediatR;

namespace SFA.DAS.Aodp.Application.Queries.Application.Application;

public class GetApplicationByIdQuery : IRequest<BaseMediatrResponse<GetApplicationByIdQueryResponse>>
{
    public GetApplicationByIdQuery(Guid applicationId)
    {
        ApplicationId = applicationId;
    }
    public Guid ApplicationId { get; set; }
}
